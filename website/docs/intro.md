---
id: intro
title: Introduction
sidebar_position: 1
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid, InlineSnippet } from '@site/src/components/DocsComponents';
import CodeWindow from '@site/src/components/CodeWindow';

<DocsWrapper>

<DocSection 
  title="EasyValidate" 
  subtitle="High-performance attribute-based validation for .NET with compile-time source generation and zero reflection overhead."
  icon="⚡"
>

<CodeWindow 
  windows={[
  {
  fileName:"User.cs",
  language:"csharp",
  snipt:`public partial class User
{
    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }
    
    [Range(18, 99)]
    public int Age { get; set; }
}

// Generated at compile-time - no reflection!
var user = new User { Email = "test@example.com", Age = 25 };
var result = user.Validate();`
  }
  ]}

  showCopyButton={true}
/>

</DocSection>
## Key Features

<FeatureGrid>
  <FeatureCard
    icon="🚀"
    title="Source Generation"
    description="Validation code generated at compile-time, no reflection"
    color="primary"
  />
  <FeatureCard
    icon="📝"
    title="Attribute-Based"
    description="Clean, declarative validation using simple attributes"
    color="secondary"
  />
  <FeatureCard
    icon="🔧"
    title="IDE Integration"
    description="IntelliSense support and compile-time error detection"
    color="primary"
  />
  <FeatureCard
    icon="📦"
    title="30+ Attributes"
    description="Comprehensive validation covering all common scenarios"
    color="secondary"
  />
</FeatureGrid>

## Getting Started

<InfoBox type="success" title="Quick Links">
**[📦 Installation](installation.md)** - Install packages and setup<br/>
**[⚡ Quick Start](quickstart.md)** - Learn with examples<br/>
**[📚 Attributes](attributes.md)** - Browse all validation attributes
</InfoBox>

</DocsWrapper>
