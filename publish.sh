#!/bin/bash
set -e

echo "Cleaning project..."
dotnet clean Src/EasyValidate/EasyValidate.csproj
dotnet clean Src/EasyValidate.Generator/EasyValidate.Generator.csproj
dotnet clean Src/EasyValidate.Attributes/EasyValidate.Attributes.csproj
dotnet clean Src/EasyValidate.Abstractions/EasyValidate.Abstractions.csproj
dotnet clean Src/EasyValidate.Fixers/EasyValidate.Fixers.csproj

echo "Removing bin and obj directories..."
rm -rf Src/EasyValidate.Abstractions/bin Src/EasyValidate.Abstractions/obj
rm -rf Src/EasyValidate.Attributes/bin Src/EasyValidate.Attributes/obj
rm -rf Src/EasyValidate.Fixers/bin Src/EasyValidate.Fixers/obj
rm -rf Src/EasyValidate.Generator/bin Src/EasyValidate.Generator/obj
rm -rf Src/EasyValidate/bin Src/EasyValidate/obj

# 4. remove local source 
if  dotnet nuget list source | grep -q "easy_validate_local"; then
  echo "Removing old easy_validate_local NuGet source..."
  dotnet nuget remove source easy_validate_local
  rm -rf local-nuget
fi

echo "Restoring dependencies..."
dotnet restore Src/EasyValidate.Abstractions/EasyValidate.Abstractions.csproj
dotnet restore Src/EasyValidate.Attributes/EasyValidate.Attributes.csproj
dotnet restore Src/EasyValidate.Fixers/EasyValidate.Fixers.csproj
dotnet restore Src/EasyValidate.Generator/EasyValidate.Generator.csproj
dotnet restore Src/EasyValidate/EasyValidate.csproj


echo "Building EasyValidate.Abstractions..."
dotnet build -c Release Src/EasyValidate.Abstractions/EasyValidate.Abstractions.csproj
echo "Publishing EasyValidate.Abstractions to nuget.org..."
dotnet nuget push Src/EasyValidate.Abstractions/bin/Release/EasyValidate.Abstractions.*.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json



echo "Building EasyValidate.Attributes..."
dotnet build -c Release Src/EasyValidate.Attributes/EasyValidate.Attributes.csproj
echo "Publishing EasyValidate.Attributes to nuget.org..."
dotnet nuget push Src/EasyValidate.Attributes/bin/Release/EasyValidate.Attributes.*.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json



echo "Building EasyValidate.Generator..."
dotnet build -c Release Src/EasyValidate.Generator/EasyValidate.Generator.csproj
echo "Publishing EasyValidate.Generator to nuget.org..."
dotnet nuget push Src/EasyValidate.Generator/bin/Release/EasyValidate.Generator.*.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json


echo "Building EasyValidate..."
dotnet build -c Release Src/EasyValidate/EasyValidate.csproj
echo "Publishing EasyValidate to nuget.org..."
dotnet nuget push Src/EasyValidate/bin/Release/EasyValidate.*.nupkg --api-key $NUGET_API_KEY --source https://api.nuget.org/v3/index.json

