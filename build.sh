#!/usr/bin/env bash
set -e

echo "Building CredentialCheck..."

dotnet restore
dotnet build --no-restore

echo "Build successful."