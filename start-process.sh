#!/bin/bash 
container_name=carrinho-api-v1
aws_default_region=us-east-1
aws_account_id=590184025050
aws_registry_name=carrinho-api-prod

#ECR Login
aws ecr get-login-password --region $aws_default_region | docker login --username AWS --password-stdin $aws_account_id.dkr.ecr.$aws_default_region.amazonaws.com

#Pulling image from ECR
docker pull $aws_account_id.dkr.ecr.$aws_default_region.amazonaws.com/$aws_registry_name:latest

##Changing image tag
docker image tag $aws_account_id.dkr.ecr.$aws_default_region.amazonaws.com/$aws_registry_name:latest $container_name:latest

#stop and remove the current container docker rm -f $container_name
docker stop $container_name
docker rm $container_name
#Creating and starting a docker container using a new image
docker run -d -p 3006:80 --name $container_name $container_name:latest