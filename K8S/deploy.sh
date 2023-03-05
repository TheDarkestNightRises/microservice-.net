#!/bin/bash

#Services
kubectl apply -f commands-depl.yaml
kubectl apply -f platforms-depl.yaml
kubectl apply -f platforms-np-srv.yaml
#Ingress nginx
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.6.4/deploy/static/provider/cloud/deploy.yaml
kubectl apply -f ingress-srv.yaml
#Mssql
kubectl apply -f local-pvc.yaml
kubectl apply -f mssql-plat-depl.yaml
# Dont forget to kubectl create secret generic mssql --from-literal=SA_PASSWORD="password" 