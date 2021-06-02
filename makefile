IMAGE_REG ?= ghcr.io
IMAGE_REPO ?= benc-uk/dapr-dotnet-starter
IMAGE_TAG ?= latest

SENDER_DIR := sender
RECEIVER_DIR := receiver

.PHONY: help lint lint-fix images push run-sender run-receiver
.DEFAULT_GOAL := help

help: ## 💬 This help message
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

lint: ## 🔎 Lint & format, will not fix but sets exit code on error 
	@dotnet format --help > /dev/null 2> /dev/null || dotnet tool install --global dotnet-format
	dotnet format --check --fix-style info --fix-analyzers info --fix-whitespace project.sln

lint-fix: ## 📜 Lint & format, will try to fix errors and modify code 
	@dotnet format --help > /dev/null 2> /dev/null || dotnet tool install --global dotnet-format
	dotnet format --fix-style info --fix-analyzers info --fix-whitespace project.sln

images: ## 🔨 Build container images 
	cd $(SENDER_DIR); docker build . --tag $(IMAGE_REG)/$(IMAGE_REPO)/sender:$(IMAGE_TAG)
	cd $(RECEIVER_DIR); docker build . --tag $(IMAGE_REG)/$(IMAGE_REPO)/receiver:$(IMAGE_TAG)

push: ## 📤 Push container images to registry 
	docker push $(IMAGE_REG)/$(IMAGE_REPO)/sender:$(IMAGE_TAG)
	docker push $(IMAGE_REG)/$(IMAGE_REPO)/receiver:$(IMAGE_TAG)

run-sender: ## 🥎 Run sender locally using Dapr & Dotnet CLI
	dapr run --app-id sender -- dotnet run -p $(SENDER_DIR)

run-receiver: ## 🧤 Run receiver locally using Dapr & Dotnet CLI
	dapr run --app-id receiver --app-port 5000 -- dotnet run -p $(RECEIVER_DIR)

restore: ## 💫 Dotnet restore
	dotnet restore project.sln

deploy: ## 🚀 Deploy to Kubernetes
	helm repo add bitnami https://charts.bitnami.com/bitnami
	helm upgrade --install redis bitnami/redis -f ./kubernetes/redis/values.yaml
	kubectl apply -f ./kubernetes

undeploy: ## 💥 Remove from Kubernetes
	helm delete redis
	kubectl delete -f ./kubernetes

clean: ## 🧹 Clean up project
	rm -rf $(SENDER_DIR)/bin
	rm -rf $(SENDER_DIR)/obj
	rm -rf $(RECEIVER_DIR)/bin
	rm -rf $(RECEIVER_DIR)/obj
