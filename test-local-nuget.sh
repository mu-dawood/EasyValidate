#!/bin/bash
set -e


# 1. Create local NuGet source directory if it doesn't exist
LOCAL_NUGET=./local-nuget
mkdir -p "$LOCAL_NUGET"

# Clear only EasyValidate and EasyValidate.Analyzers from local NuGet cache
echo "Clearing EasyValidate and EasyValidate.Analyzers from local NuGet cache..."
rm -rf ~/.nuget/packages/easyvalidate ~/.nuget/packages/easyvalidate.analyzers 2>/dev/null || true


# 2. Build the EasyValidate.Analyzers project first (so the DLL is available for embedding)
echo "Restore"
dotnet restore

# 3. Build and pack the EasyValidate project (includes embedded analyzers)
echo "Building and packing EasyValidate..."
dotnet build -c Release EasyValidate/EasyValidate.csproj
dotnet pack -c Release EasyValidate/EasyValidate.csproj


# 4. Add the local source to NuGet config (if not already added)
if  dotnet nuget list source | grep -q "easy_validate_local"; then
  echo "Removing old easy_validate_local NuGet source..."
  dotnet nuget remove source easy_validate_local
fi

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
