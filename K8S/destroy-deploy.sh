#!/bin/bash

kubectl delete deployment commands-depl
kubectl delete deployment mssql-depl
kubectl delete deployment platforms-depl
kubectl delete services commands-clusterip-srv
kubectl delete service platforms-clusterip-srv
kubectl delete service mssql-clusterip-srv
kubectl delete service platformnpservice-srv
kubectl delete service mssql-loadbalancer
kubectl delete deployment ingress-nginx-controller -n ingress-nginx