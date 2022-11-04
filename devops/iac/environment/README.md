[back](../README.md)
# Terraform infrastructure set up script
This Terraform script sets up a web app structure in the cloud services Azure and Mongo Atlas.

General description of structure this script sets up in Azure/Mongo Atlas:
* A backend app in an app service plan.
* A frontend app as a static web app.
* A proxy using Azure functions to connect backend and frontend.
* A mongo database in mongo atlas.

The script is used in our pipelines but can be developed safely by using the\
commands defined below.

# prerequisites for using script
Must be logged in with `az login`.

# commands
### init
terraform init -var-file=subscriptions.tfvars -backend-config="key=moollocaldev1/terraform.tfstate" -reconfigure
### plan
terraform plan -var-file=subscriptions.tfvars -var 'infrastructure_name=moollocaldev1'
### apply
terraform apply -var-file=subscriptions.tfvars -var 'infrastructure_name=moollocaldev1'