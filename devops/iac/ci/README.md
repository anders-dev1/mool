[back](../README.md)
# Terraform CI infrastructure set up script
This script sets up the infrastructure for the CI(build) pipeline. \
The only resources it sets up is a Azure Container Registry to save the CI images in.

The script is used in our pipelines but can be developed safely by using the commands defined below.

# prerequisites for using script
Must be logged in with `az login`.

# commands
### init
terraform init -var-file=subscriptions.tfvars -backend-config="key=moolcidev/terraform.tfstate" -reconfigure
### plan
terraform plan -var-file=subscriptions.tfvars -var 'infrastructure_name=moolcidev'
### apply
terraform apply -var-file=subscriptions.tfvars -var 'infrastructure_name=moolcidev'