# Deploy to Kubernetes

Work from the the kubernetes directory
```
cd kubernetes
```

## 1️⃣ Deploy Redis
```
helm repo add bitnami https://charts.bitnami.com/bitnami
helm install redis bitnami/redis -f ./redis/values.yaml
```

## 2️⃣ Deploy Dapr Component(s)
```
kubectl apply -f statestore.yaml
```

## 3️⃣ Deploy Sender & Receiver

**👁‍🗨 Note.** The receiver deployment does not expose any container ports (the process is still listening on port 5000), and no Kubernetes service is used, yet the sender can still invoke it via the Dapr data plane

```
kubectl apply -f receiver.yaml -f sender.yaml
```