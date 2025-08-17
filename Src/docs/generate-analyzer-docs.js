#!/usr/bin/env node
// Script to generate analyzer documentation from exported data

const fs = require('fs');
const path = require('path');

// Import the analyzer data (this would need to be updated to import the actual generated data)
// For now, we'll read it from the generated TypeScript file and parse the relevant sections

const attributesFile = path.join(__dirname, '../website/src/data/attributes.ts');
const docsFile = path.join(__dirname, 'analyzers.md');

function extractAnalyzerData() {
  const content = fs.readFileSync(attributesFile, 'utf8');
  
  // Extract the analyzer section data
  const analyzerSectionMatch = content.match(/export const analyzerSection: AnalyzerSection \| null = ([\s\S]*?);[\s]*export const allSections/);
  
  if (!analyzerSectionMatch) {
    console.error('Could not find analyzer section in attributes.ts');
    return null;
  }
  
  try {
    // Clean up the extracted data to make it valid JSON
    let analyzerData = analyzerSectionMatch[1]
      .replace(/SearchIcon/g, '"SearchIcon"')  // Replace icon reference with string
      .replace(/,\s*}/g, '}')  // Remove trailing commas
      .replace(/,\s*]/g, ']'); // Remove trailing commas in arrays
    
    return JSON.parse(analyzerData);
  } catch (error) {
    console.error('Error parsing analyzer data:', error);
    return null;
  }
}

function generateAnalyzerDocs(analyzerSection) {
  if (!analyzerSection || !analyzerSection.analyzers) {
    console.error('No analyzer data found');
    return;
  }

  const analyzers = analyzerSection.analyzers;
  
  let docs = `---
title: EasyValidate Analyzers
layout: default
---

# Code Analyzers

${analyzerSection.description}

## Overview

The analyzer package provides static code analysis rules that help catch validation issues at compile time. These rules are designed to:

- Ensure proper implementation of custom validation attributes
- Validate correct usage of validation attributes
- Provide immediate feedback in the IDE
- Prevent common validation configuration errors

## Analyzer Rules

`;

  // Generate documentation for each analyzer
  analyzers.forEach(analyzer => {
    docs += `### ${analyzer.id}: ${analyzer.title}

**Category:** ${analyzer.category}  
**Severity:** ${analyzer.severity}  
**Source:** ${analyzer.source}

**Description:**  
${analyzer.description || 'This rule ensures proper usage of validation attributes and methods.'}

**Message:**  
${analyzer.messageFormat}

`;

    // Add specific examples based on analyzer ID
    if (analyzer.id === 'VAL001') {
      docs += `**Example of violation:**
\`\`\`csharp
public class CustomValidationAttribute : StringValidationAttributeBase
{
    // ❌ Wrong signature - missing parameters
    public override AttributeResult Validate()
    {
        return new AttributeResult();
    }
}
\`\`\`

**Correct implementation:**
\`\`\`csharp
public class CustomValidationAttribute : StringValidationAttributeBase
{
    // ✅ Correct signature
    public override AttributeResult Validate(string value, Type propertyType)
    {
        return new AttributeResult();
    }
}
\`\`\`

`;
    } else if (analyzer.id === 'VAL002') {
      docs += `**Example of violation:**
\`\`\`csharp
public class Example
{
    // ❌ Wrong usage - applying string validation to numeric property
    [EmailAddress]
    public int Number { get; set; }
}
\`\`\`

**Correct implementation:**
\`\`\`csharp
public class Example
{
    // ✅ Correct usage - string validation on string property
    [EmailAddress]
    public string Email { get; set; }
    
    // ✅ Correct usage - numeric validation on numeric property
    [Range(0, 100)]
    public int Number { get; set; }
}
\`\`\`

`;
    } else if (analyzer.id === 'VAL004') {
      docs += `**Example of violation:**
\`\`\`csharp
public class Example
{
    // ❌ Invalid - power must be greater than 1
    [PowerOf(1)]
    public int Value { get; set; }
    
    // ❌ Invalid - power must be greater than 1
    [PowerOf(0)]
    public int AnotherValue { get; set; }
}
\`\`\`

**Correct implementation:**
\`\`\`csharp
public class Example
{
    // ✅ Valid - power of 2
    [PowerOf(2)]
    public int Value { get; set; }
    
    // ✅ Valid - power of 3
    [PowerOf(3)]
    public int CubicValue { get; set; }
}
\`\`\`

`;
    } else if (analyzer.id === 'VAL005') {
      docs += `**Example of violation:**
\`\`\`csharp
public class Example
{
    // ❌ Invalid - cannot divide by zero
    [DivisibleBy(0)]
    public int Value { get; set; }
}
\`\`\`

**Correct implementation:**
\`\`\`csharp
public class Example
{
    // ✅ Valid - divisible by 2 (even numbers)
    [DivisibleBy(2)]
    public int EvenValue { get; set; }
    
    // ✅ Valid - divisible by 5
    [DivisibleBy(5)]
    public int MultipleOfFive { get; set; }
}
\`\`\`

`;
    } else if (analyzer.id === 'VAL010') {
      docs += `**Example of violation:**
\`\`\`csharp
public class Example
{
    // ❌ Invalid - string validation on numeric collection
    [Contains("text")]
    public List<int> Numbers { get; set; }
}
\`\`\`

**Correct implementation:**
\`\`\`csharp
public class Example
{
    // ✅ Valid - string validation on string collection
    [Contains("text")]
    public List<string> Texts { get; set; }
    
    // ✅ Valid - numeric validation on numeric collection
    [Contains(42)]
    public List<int> Numbers { get; set; }
}
\`\`\`

`;
    }
  });

  // Add the rest of the documentation (IDE integration, configuration, etc.)
  docs += `## IDE Integration

The analyzers integrate seamlessly with popular IDEs:

### Visual Studio
- Real-time error highlighting
- Quick fixes and suggestions
- Error list integration
- IntelliSense support

### Visual Studio Code
- Error squiggles in editor
- Problems panel integration
- Hover tooltips with rule details

### JetBrains Rider
- Inspection highlighting
- Solution-wide analysis
- Quick-fix suggestions

## Configuration

You can configure analyzer behavior in your project file:

\`\`\`xml
<PropertyGroup>
  <!-- Disable specific analyzer rules -->
  <NoWarn>$(NoWarn);VAL004</NoWarn>
  
  <!-- Treat analyzers as warnings instead of errors -->
  <WarningsAsErrors />
  <WarningsNotAsErrors>VAL001;VAL002</WarningsNotAsErrors>
</PropertyGroup>
\`\`\`

## EditorConfig Support

Configure analyzer severity using EditorConfig:

\`\`\`ini
# .editorconfig
[*.cs]
# Set specific rule severity
dotnet_diagnostic.VAL001.severity = error
dotnet_diagnostic.VAL002.severity = warning
dotnet_diagnostic.VAL004.severity = suggestion
\`\`\`

## Suppression

Suppress analyzer warnings when needed:

\`\`\`csharp
public class Example
{
    // Suppress specific rule for this property
#pragma warning disable VAL004
    [PowerOf(1)] // Suppressed for this specific case
    public int SpecialValue { get; set; }
#pragma warning restore VAL004
}
\`\`\`

## Troubleshooting

### Analyzers Not Running

1. Ensure the analyzer package is properly referenced:
   \`\`\`xml
   <PackageReference Include="EasyValidate.Analyzers" Version="1.0.0">
     <PrivateAssets>all</PrivateAssets>
     <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
   </PackageReference>
   \`\`\`

2. Clean and rebuild the solution
3. Restart the IDE
4. Check the Error List / Problems panel for analyzer messages

### Performance Issues

If analyzers are causing performance issues:

1. Disable specific rules that are not needed
2. Use solution-wide analysis sparingly
3. Configure analyzer execution in larger solutions

## Summary

| Rule ID | Title | Category | Severity |
|---------|-------|----------|----------|
${analyzers.map(a => `| ${a.id} | ${a.title} | ${a.category} | ${a.severity} |`).join('\n')}

---

For more information about extending EasyValidate with custom analyzers, see the [Extending EasyValidate](index.md#extending-easyvalidate) section.
`;

  return docs;
}

function main() {
  console.log('📖 Generating analyzer documentation from exported data...');
  
  const analyzerData = extractAnalyzerData();
  if (!analyzerData) {
    process.exit(1);
  }
  
  const docs = generateAnalyzerDocs(analyzerData);
  fs.writeFileSync(docsFile, docs, 'utf8');
  
  console.log(`✅ Generated analyzer documentation at: ${docsFile}`);
  console.log(`📊 Documented ${analyzerData.analyzers.length} analyzer rules`);
}

if (require.main === module) {
  main();
}

module.exports = { extractAnalyzerData, generateAnalyzerDocs };
