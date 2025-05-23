---
id: installation
title: Installation
sidebar_position: 2
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid, InlineSnippet } from '@site/src/components/DocsComponents';

<DocsWrapper>

# Installation Guide

<DocSection 
  title="Getting Started with EasyValidate" 
  subtitle="EasyValidate is distributed through NuGet and consists of two main packages that work together to provide both runtime validation and compile-time analysis."
  icon="📦"
  background="gradient"
>

<FeatureGrid>
  <FeatureCard
    icon="⚡"
    title="Core Library"
    description="Runtime validation attributes and base classes"
    color="primary"
  />
  <FeatureCard
    icon="🔧"
    title="Analyzers"
    description="Compile-time source generation and IDE support"
    color="secondary"
  />
</FeatureGrid>

</DocSection>

## Core Package

<DocSection 
  title="EasyValidate" 
  subtitle="The main EasyValidate package contains all validation attributes, base classes, and runtime validation logic."
  icon="📚"
>

<InfoBox type="note" title="Required Package">
This is the essential package that contains all validation attributes and core functionality.
</InfoBox>

### Package Manager Console
```shell
Install-Package EasyValidate
```

### .NET CLI
```shell
dotnet add package EasyValidate
```

### PackageReference (csproj)
```xml
<PackageReference Include="EasyValidate" Version="1.0.0" />
```

</DocSection>

## Analyzer Package (Recommended)

<DocSection 
  title="EasyValidate.Analyzers" 
  subtitle="The analyzer package provides compile-time validation, IDE support, and source generation for optimal performance."
  icon="🔧"
>

<InfoBox type="success" title="Recommended">
Installing the analyzer package is highly recommended for the best development experience and performance.
</InfoBox>

### Package Manager Console
```shell
Install-Package EasyValidate.Analyzers
```

### .NET CLI
```shell
dotnet add package EasyValidate.Analyzers
```

### PackageReference (csproj)
```xml
<PackageReference Include="EasyValidate.Analyzers" Version="1.0.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```

</DocSection>

## System Requirements

- **.NET Standard 2.0** or higher
- **C# 8.0** or later (for nullable reference types support)
- **Visual Studio 2019/2022** or **VS Code** (for best IDE experience with analyzers)

## Supported Frameworks

- .NET 5.0+
- .NET Core 2.0+
- .NET Framework 4.6.1+
- Xamarin
- Unity (2021.2+)

## Verification

After installation, verify that EasyValidate is working by creating a simple model:

<InlineSnippet language="csharp">{`
using EasyValidate.Attributes;

public class User
{
    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }
    
    [Range(18, 99)]
    public int Age { get; set; }
}
`}</InlineSnippet>

If the analyzer package is installed correctly, you should see:
- IntelliSense support for validation attributes
- Compile-time warnings for incorrect attribute usage
- Auto-generated validation methods

## Troubleshooting

### Analyzer Not Working
If the analyzers aren't providing feedback:
1. Restart Visual Studio/VS Code
2. Clean and rebuild your solution
3. Ensure the analyzer package is properly referenced
4. Check that your project targets a supported framework

### Package Conflicts
If you encounter version conflicts:
1. Use the same version for both packages
2. Check your project's dependency tree
3. Consider using `<PackageReference Update="...">` for transitive dependencies

---

**Next Steps:** See the [Quick Start Guide](quickstart.md) to begin using EasyValidate in your project.

</DocsWrapper>
