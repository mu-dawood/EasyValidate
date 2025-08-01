#!/bin/bash
set -e

echo "Cleaning project..."
dotnet clean EasyValidate/EasyValidate.csproj
dotnet clean EasyValidate.Core/EasyValidate.Core.csproj
dotnet clean EasyValidate.Analyzers/EasyValidate.Analyzers.csproj
dotnet clean EasyValidate.Fixers/EasyValidate.Fixers.csproj

echo "Removing bin and obj directories..."
rm -rf EasyValidate.Core/bin EasyValidate.Core/obj
rm -rf EasyValidate.Analyzers/bin EasyValidate.Analyzers/obj
rm -rf EasyValidate.Fixers/bin EasyValidate.Fixers/obj
rm -rf EasyValidate/bin EasyValidate/obj

# 4. remove local source 
if  dotnet nuget list source | grep -q "easy_validate_local"; then
  echo "Removing old easy_validate_local NuGet source..."
  dotnet nuget remove source easy_validate_local
  rm -rf local-nuget
fi

echo "Restoring dependencies..."
dotnet restore EasyValidate.Core/EasyValidate.Core.csproj
dotnet restore EasyValidate.Analyzers/EasyValidate.Analyzers.csproj
dotnet restore EasyValidate.Fixers/EasyValidate.Fixers.csproj
dotnet restore EasyValidate/EasyValidate.csproj


echo "Building EasyValidate..."
dotnet build -c Release EasyValidate/EasyValidate.csproj
dotnet pack -c Release EasyValidate/EasyValidate.csproj

echo "Publishing to nuget.org..."
dotnet nuget push EasyValidate/bin/Release/EasyValidate.*.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json

