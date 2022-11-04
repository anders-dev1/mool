#region Terraform configuration
// Terraform
terraform {
  required_providers {
    mongodbatlas = {
      source  = "mongodb/mongodbatlas"
      version = "1.3.0"
    }
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.29.0"
    }
  }
  backend "azurerm" {
    // The backend section does not support variables. The storage key is defined when initialising Terraform.
    // The storage key depends on the environment and can be found in the readme.md file as a terraform init command parameter.
    resource_group_name  = "general-directory-resources"
    storage_account_name = "tfstates130465"
    container_name       = "states"
  }
}
#endregion

#region Variables
variable "infrastructure_name" {
  description = "The name of the environment (development, staging, production etc.)"
}

variable "azurerm_mongo_key_vault_name" {
  description = "The name of the key vault where the MongoDB Atlas private API key is stored."
}

variable "azurerm_mongo_key_vault_resource_group_name" {
  description = "The name of the resource group that contains the key vault where the MongoDB Atlas private API key is stored."
}

variable "azurerm_mongo_private_key_secret_name" {
  description = "The name of the secret in the key vault which contains the MongoDB Atlas private API key."
}

variable "azurerm_subscription_id" {
  description = "The ID of the Azure subscription in which the TerraForm state will be saved."
}

variable "azurerm_subscription_tenant_id" {
  description = "The ID of the Azure subscription tenant in which the TerraForm state will be saved."
}

variable "mongodbatlas_public_key" {
  description = "The public key from key-pair generated in Atlas' access manager."
}

variable "mongodbatlas_org_id" {
  description = "The organisation ID available in Atlas' settings."
}

variable "mongodbatlas_api_key_id" {
  description = "The ID of the API key to use."
}
#endregion

#region Locals
locals {
  //The unique name of this infrastructure. MUST BE GLOBALLY UNIQUE ACROSS ALL OF AZURE'S RESOURCES!
  backend_name                     = "${var.infrastructure_name}-backend"
  backend_url_no_protocol_prefix   = "${local.backend_name}.azurewebsites.net"
  backend_url_with_protocol_prefix = "https://${local.backend_name}.azurewebsites.net"
  location                         = "West Europe"
}
#endregion

#region Data
// Data
data "azurerm_key_vault" "kv" {
  name                = var.azurerm_mongo_key_vault_name
  resource_group_name = var.azurerm_mongo_key_vault_resource_group_name
}

data "azurerm_key_vault_secret" "mongo_private_key" {
  name         = var.azurerm_mongo_private_key_secret_name
  key_vault_id = data.azurerm_key_vault.kv.id
}
#endregion

#region Providers
// Providers
provider "azurerm" {
  subscription_id = var.azurerm_subscription_id
  tenant_id       = var.azurerm_subscription_tenant_id
  features {}
}

provider "mongodbatlas" {
  public_key  = var.mongodbatlas_public_key
  private_key = data.azurerm_key_vault_secret.mongo_private_key.value
}
#endregion

#region Resources
#region Mongo
resource "random_password" "mongo_user_password" {
  length  = 16
  special = false
}

resource "mongodbatlas_project" "main" {
  name   = var.infrastructure_name
  org_id = var.mongodbatlas_org_id

  api_keys {
    api_key_id = var.mongodbatlas_api_key_id
    role_names = ["GROUP_OWNER"]
  }
}

resource "mongodbatlas_project_ip_access_list" "main" {
  project_id = mongodbatlas_project.main.id
  cidr_block = "0.0.0.0/0"
}

resource "mongodbatlas_cluster" "main" {
  project_id = mongodbatlas_project.main.id
  name       = var.infrastructure_name

  # Provider Settings "block"
  provider_name               = "TENANT"
  backing_provider_name       = "AZURE"
  provider_region_name        = "EUROPE_NORTH"
  provider_instance_size_name = "M0"
}

resource "mongodbatlas_database_user" "user" {
  project_id         = mongodbatlas_project.main.id
  username           = var.infrastructure_name
  password           = random_password.mongo_user_password.result
  auth_database_name = "admin"

  roles {
    role_name     = "readWrite"
    database_name = var.infrastructure_name
  }
}
#endregion

// Azure
resource "azurerm_resource_group" "main" {
  name     = var.infrastructure_name
  location = local.location
}

#region Application Insights
resource "azurerm_log_analytics_workspace" "main" {
  location            = local.location
  name                = var.infrastructure_name
  resource_group_name = azurerm_resource_group.main.name
  sku                 = "PerGB2018"
}

resource "azurerm_application_insights" "backend" {
  name                = "${var.infrastructure_name}-backend"
  application_type    = "web"
  location            = local.location
  resource_group_name = azurerm_resource_group.main.name
  workspace_id        = azurerm_log_analytics_workspace.main.id
}

resource "azurerm_application_insights" "frontend" {
  name                = "${var.infrastructure_name}-frontend"
  application_type    = "web"
  location            = local.location
  resource_group_name = azurerm_resource_group.main.name
  workspace_id        = azurerm_log_analytics_workspace.main.id
}
#endregion

resource "azurerm_container_registry" "main" {
  name                = var.infrastructure_name
  resource_group_name = azurerm_resource_group.main.name
  location            = local.location
  sku                 = "Basic"
  admin_enabled       = true

  identity {
    type = "SystemAssigned"
  }
}

#region Services
resource "random_uuid" "jwt_secret" {
}

resource "azurerm_service_plan" "main" {
  name                = var.infrastructure_name
  resource_group_name = azurerm_resource_group.main.name
  location            = local.location
  os_type             = "Linux"
  sku_name            = "B1"
}

resource "azurerm_linux_web_app" "backend" {
  name                = "${local.backend_name}"
  resource_group_name = azurerm_resource_group.main.name
  location            = local.location
  service_plan_id     = azurerm_service_plan.main.id

  app_settings = {
    # ASPNETCORE_HOSTINGSTARTUPASSEMBLIES = "Microsoft.AspNetCore.ApplicationInsights.HostingStartup"
    APPINSIGHTS_INSTRUMENTATIONKEY          = azurerm_application_insights.backend.instrumentation_key
    MongoDB__DatabaseName                   = var.infrastructure_name
    MongoDB__MongoDbSrvConnection__Address  = "${mongodbatlas_cluster.main.srv_address}/${var.infrastructure_name}"
    MongoDB__MongoDbSrvConnection__Username = mongodbatlas_database_user.user.username
    MongoDB__MongoDbSrvConnection__Password = mongodbatlas_database_user.user.password
    JWT__Secret                             = random_uuid.jwt_secret.result
    Environment__Url                        = local.backend_url_no_protocol_prefix
    WEBSITE_RUN_FROM_PACKAGE                = 1
  }

  site_config {
    container_registry_use_managed_identity = true

    application_stack {
      docker_image     = "${azurerm_container_registry.main.login_server}/moolbackend"
      docker_image_tag = "latest"
    }
  }

  identity {
    type = "SystemAssigned"
  }

  logs {
    http_logs {
      file_system {
        retention_in_mb   = 35
        retention_in_days = 0
      }
    }
  }

  lifecycle {
    ignore_changes = [
      # These settings are needed for initial deployment so the web app understand that it is a container app, otherwise it will just host default Azure web app.
      # The settings are updated via the pipeline when new images are deployed.
      site_config[0].application_stack[0].docker_image,
      site_config[0].application_stack[0].docker_image_tag
    ]
  }
}

resource "azurerm_role_assignment" "backend" {
  role_definition_name = "AcrPull"
  scope                = azurerm_container_registry.main.id
  principal_id         = azurerm_linux_web_app.backend.identity[0].principal_id
}

resource "azurerm_linux_web_app" "frontend" {
  name                = "${var.infrastructure_name}-frontend"
  resource_group_name = azurerm_resource_group.main.name
  location            = local.location
  service_plan_id     = azurerm_service_plan.main.id

  app_settings = {
    # ASPNETCORE_HOSTINGSTARTUPASSEMBLIES = "Microsoft.AspNetCore.ApplicationInsights.HostingStartup"
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.frontend.instrumentation_key
    proxy_pass                     = local.backend_url_with_protocol_prefix
  }

  site_config {
    container_registry_use_managed_identity = true

    application_stack {
      docker_image     = "${azurerm_container_registry.main.login_server}/moolfrontend"
      docker_image_tag = "latest"
    }
  }

  identity {
    type = "SystemAssigned"
  }

  logs {
    http_logs {
      file_system {
        retention_in_mb   = 35
        retention_in_days = 0
      }
    }
  }

  lifecycle {
    ignore_changes = [
      # These settings are needed for initial deployment so the web app understand that it is a container app, otherwise it will just host default Azure web app.
      # The settings are updated via the pipeline when new images are deployed.
      site_config[0].application_stack[0].docker_image,
      site_config[0].application_stack[0].docker_image_tag
    ]
  }
}

resource "azurerm_role_assignment" "frontend" {
  role_definition_name = "AcrPull"
  scope                = azurerm_container_registry.main.id
  principal_id         = azurerm_linux_web_app.frontend.identity[0].principal_id
}
#endregion

#region output
output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

#output "backend_web_app_resource_name" {
#  value = azurerm_linux_web_app.backend.name
#}
#endregion
