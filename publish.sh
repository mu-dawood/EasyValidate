#!/bin/bash
set -e


echo "Building EasyValidate..."
dotnet build -c Release EasyValidate/EasyValidate.csproj
dotnet pack -c Release EasyValidate/EasyValidate.csproj

echo "Publishing to nuget.org..."
dotnet nuget push EasyValidate/bin/Release/EasyValidate.*.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json

