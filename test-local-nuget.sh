#!/bin/bash

set -e

# Increment build version in Directory.Build.props before any action
echo "Incrementing build version in Directory.Build.props..."
VERSION_FILE="Src/Directory.Build.props"

# Extract old version
OLD_VERSION=$(awk -F'[<>]' '/<Version>/ {print $3}' "$VERSION_FILE")

IFS='.' read -r MAJOR MINOR PATCH <<< "$OLD_VERSION"
NEW_PATCH=$((PATCH + 1))
NEW_VERSION="$MAJOR.$MINOR.$NEW_PATCH"

# sed inline replacement (cross-platform)
# macOS requires '' after -i, Linux does not
if sed --version >/dev/null 2>&1; then
  # GNU sed (Linux)
  sed -i "s|<Version>$OLD_VERSION</Version>|<Version>$NEW_VERSION</Version>|" "$VERSION_FILE"
else
  # BSD sed (macOS)
  sed -i '' "s|<Version>$OLD_VERSION</Version>|<Version>$NEW_VERSION</Version>|" "$VERSION_FILE"
fi




# Clear only EasyValidate and EasyValidate.Analyzers from local NuGet cache
echo "Clearing EasyValidate and EasyValidate.Analyzers from local NuGet cache..."
rm -rf ~/.nuget/packages/easyvalidate ~/.nuget/packages/easyvalidate.analyzers 2>/dev/null || true


echo "Cleaning project..."
dotnet clean Src/EasyValidate/EasyValidate.csproj
dotnet clean Src/EasyValidate.Generator/EasyValidate.Generator.csproj
dotnet clean Src/EasyValidate.Attributes/EasyValidate.Attributes.csproj
dotnet clean Src/EasyValidate.Abstractions/EasyValidate.Abstractions.csproj
dotnet clean Src/EasyValidate.Fixers/EasyValidate.Fixers.csproj

echo "Removing bin and obj directories..."
rm -rf Src/EasyValidate.Abstractions/bin EasyValidate.Abstractions/obj
rm -rf Src/EasyValidate.Attributes/bin EasyValidate.Attributes/obj
rm -rf Src/EasyValidate.Fixers/bin EasyValidate.Fixers/obj
rm -rf Src/EasyValidate.Generator/bin EasyValidate.Generator/obj
rm -rf Src/EasyValidate/bin EasyValidate/obj


echo "Removing EasyValidate from test projects if present..."
for PROJ in Test/ConsoleTest/ConsoleTest.csproj Test/EasyValidate.Test/EasyValidate.Test.csproj; do
  if dotnet list "$PROJ" package | grep -q "EasyValidate"; then
    echo "Removing EasyValidate from $PROJ..."
    dotnet package remove Src/EasyValidate --project "$PROJ" 2>/dev/null || true
  else
    echo "EasyValidate not found in $PROJ, skipping removal."
  fi
done

if  dotnet nuget list source | grep -q "easy_validate_local"; then
  echo "Removing old easy_validate_local NuGet source..."
  dotnet nuget remove source easy_validate_local
  rm -rf local-nuget
fi





# 1. Create local NuGet source directory if it doesn't exist
LOCAL_NUGET=./local-nuget
mkdir -p "$LOCAL_NUGET"

echo "Adding local NuGet source..."
dotnet nuget add source "$(pwd)/local-nuget" -n easy_validate_local

# abstractions
echo "Building EasyValidate.Abstractions"
dotnet build -c Release Src/EasyValidate.Abstractions/EasyValidate.Abstractions.csproj
AbstractionsPKG_PATH=$(ls -t Src/EasyValidate.Abstractions/bin/Release/EasyValidate.Abstractions.*.nupkg | head -n 1)
echo "Adding $AbstractionsPKG_PATH to $LOCAL_NUGET using dotnet nuget push..."
cp "$AbstractionsPKG_PATH" "$LOCAL_NUGET/"


#attributes
echo "Building EasyValidate.Attributes"
dotnet build -c Release Src/EasyValidate.Attributes/EasyValidate.Attributes.csproj
AttributesPKG_PATH=$(ls -t Src/EasyValidate.Attributes/bin/Release/EasyValidate.Attributes.*.nupkg | head -n 1)
echo "Adding $AttributesPKG_PATH to $LOCAL_NUGET using dotnet nuget push..."
cp "$AttributesPKG_PATH" "$LOCAL_NUGET/"


#generator
echo "Building EasyValidate.Generator"
dotnet build -c Release Src/EasyValidate.Generator/EasyValidate.Generator.csproj
GeneratorPKG_PATH=$(ls -t Src/EasyValidate.Generator/bin/Release/EasyValidate.Generator.*.nupkg | head -n 1)
echo "Adding $GeneratorPKG_PATH to $LOCAL_NUGET using dotnet nuget push..."
cp "$GeneratorPKG_PATH" "$LOCAL_NUGET/"


echo "Building EasyValidate..."
dotnet build -c Release Src/EasyValidate/EasyValidate.csproj

# 5. Add the EasyValidate package to the local source
PKG_PATH=$(ls -t Src/EasyValidate/bin/Release/EasyValidate.*.nupkg | head -n 1)
echo "Adding $PKG_PATH to $LOCAL_NUGET using dotnet nuget push..."
cp "$PKG_PATH" "$LOCAL_NUGET/"





# 6. Add the EasyValidate package from the local source to the ConsoleTest project

echo "Adding EasyValidate package from local source to ConsoleTest..."
dotnet add Test/ConsoleTest/ConsoleTest.csproj package EasyValidate --version $NEW_VERSION
dotnet add Test/UnitTest/UnitTest.csproj package EasyValidate --version $NEW_VERSION

echo "Done! EasyValidate has been added from your local NuGet source to ConsoleTest."
