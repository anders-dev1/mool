/*
  Sets up CI container registry.
*/

#region Terraform configuration
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.21.1"
    }
  }
  backend "azurerm" {
    resource_group_name  = "general-directory-resources"
    storage_account_name = "tfstates130465"
    container_name       = "states"
  }
}
#endregion

#region Variables
variable "infrastructure_name" {
  description = "The name of the environment (development, production)"
}

variable "azurerm_subscription_id" {
  description = "The ID of the Azure subscription in which the Terraform state will be saved."
}

variable "azurerm_tenant_id" {
  description = "The ID of the Azure tenant containing the subscription in which the Terraform state will be saved."
}
#endregion

#region Locals
locals {
  infrastructure_name         = "${var.infrastructure_name}" //The unique name of this infrastructure. MUST BE GLOBALLY UNIQUE ACROSS ALL OF AZURE'S RESOURCES!
  location                    = "West Europe"
}
#endregion

provider "azurerm" {
  subscription_id = var.azurerm_subscription_id
  tenant_id       = var.azurerm_tenant_id
  features {}
}

#region Resources
resource "azurerm_resource_group" "main" {
  name     = local.infrastructure_name
  location = local.location
}

resource "azurerm_container_registry" "main" {
  name                = local.infrastructure_name
  resource_group_name = azurerm_resource_group.main.name
  location            = local.location
  sku                 = "Basic"
  admin_enabled       = true

  identity {
    type = "SystemAssigned"
  }
}
#endregion