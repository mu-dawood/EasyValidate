#!/bin/bash

set -e

# Increment build version in Directory.Build.props before any action
echo "Incrementing build version in Directory.Build.props..."
VERSION_FILE="Directory.Build.props"
OLD_VERSION=$(awk -F'[<>]' '/<Version>/ {print $3}' "$VERSION_FILE")
IFS='.' read -r MAJOR MINOR PATCH <<< "$OLD_VERSION"
NEW_PATCH=$((PATCH + 1))
NEW_VERSION="$MAJOR.$MINOR.$NEW_PATCH"
sed -i.bak "s/<Version>$OLD_VERSION<\/Version>/<Version>$NEW_VERSION<\/Version>/" "$VERSION_FILE"
echo "Version updated: $OLD_VERSION -> $NEW_VERSION"


# 1. Create local NuGet source directory if it doesn't exist
LOCAL_NUGET=./local-nuget
mkdir -p "$LOCAL_NUGET"

# Clear only EasyValidate and EasyValidate.Analyzers from local NuGet cache
echo "Clearing EasyValidate and EasyValidate.Analyzers from local NuGet cache..."
rm -rf ~/.nuget/packages/easyvalidate ~/.nuget/packages/easyvalidate.analyzers 2>/dev/null || true


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


echo "Removing EasyValidate from test projects if present..."
for PROJ in EasyValidate.ConsoleTest/EasyValidate.ConsoleTest.csproj EasyValidate.Test/EasyValidate.Test.csproj; do
  if dotnet list "$PROJ" package | grep -q "EasyValidate"; then
    echo "Removing EasyValidate from $PROJ..."
    dotnet package remove EasyValidate --project "$PROJ" 2>/dev/null || true
  else
    echo "EasyValidate not found in $PROJ, skipping removal."
  fi
done

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


echo "Adding local NuGet source..."
dotnet nuget add source "$(cd "$LOCAL_NUGET"; pwd)" -n easy_validate_local

# 5. Add the EasyValidate package to the local source
PKG_PATH=$(ls -t EasyValidate/bin/Release/EasyValidate.*.nupkg | head -n 1)
PKG_VERSION=$(basename "$PKG_PATH" | sed -E 's/^EasyValidate\.([0-9]+\.[0-9]+\.[0-9]+).*\.nupkg$/\1/')
echo "Adding $PKG_PATH to $LOCAL_NUGET using dotnet nuget push..."
dotnet nuget push "$PKG_PATH" -s "$(cd "$LOCAL_NUGET"; pwd)"


# 6. Add the EasyValidate package from the local source to the EasyValidate.ConsoleTest project

echo "Adding EasyValidate package from local source to EasyValidate.ConsoleTest..."
dotnet add EasyValidate.ConsoleTest/EasyValidate.ConsoleTest.csproj package EasyValidate --version $PKG_VERSION
dotnet add EasyValidate.Test/EasyValidate.Test.csproj package EasyValidate --version $PKG_VERSION

echo "Done! EasyValidate has been added from your local NuGet source to EasyValidate.ConsoleTest."
