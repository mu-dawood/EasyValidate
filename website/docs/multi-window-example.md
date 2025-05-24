---
sidebar_position: 3
title: Multi-Window Example
---

import { DocSection, CodeWindow } from '../src/components/DocsComponents';

<DocSection title="Multi-tab Code Examples" icon="🧩">

## Using the Multi-Window Component

This shows how to use the new tab-based CodeWindow component.

<CodeWindow 
  windows={[
    {
      fileName: "User.cs",
      language: "csharp",
      snipt: `using EasyValidate;
using System;

public partial class User
{
    [NotEmpty(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    public string Name { get; set; }
    
    [NotEmpty(ErrorMessage = "Email is required")]
    [Email(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }
}`
    },
    {
      fileName: "Program.cs",
      language: "csharp",
      snipt: `using System;

class Program
{
    static void Main(string[] args)
    {
        var user = new User
        {
            Name = "Jo", // Too short, will fail validation
            Email = "invalid-email" // Not valid email format
        };
        
        var result = user.Validate();
        
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
            }
        }
    }
}`
    },
    {
      fileName: "Output",
      language: "bash",
      snipt: `Name: Name must be at least 3 characters
Email: Please enter a valid email`
    }
  ]}
  variant="light"
/>

## Single Tab Example

<CodeWindow 
  windows={[
    {
      fileName: "appsettings.json",
      language: "json",
      snipt: `{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ValidationSettings": {
    "EnableLocalization": true,
    "DefaultCulture": "en-US"
  }
}`
    }
  ]}
  variant="light"
/>

</DocSection>
