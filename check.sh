#!/usr/bin/env bash
set -e

echo "Building CredentialCheck..."
dotnet build

echo "Checking health endpoint..."
curl -s http://dev20:5000/health
echo