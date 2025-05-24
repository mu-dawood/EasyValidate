---
sidebar_position: 2
title: Example Usage
---

import { DocSection, CodeWindow, Window } from '../src/components/DocsComponents';

<DocSection title="Multi-tab Code Examples" icon="🧪">

This demonstrates how to use the enhanced CodeWindow with multiple tabs.

## Basic Examples

<CodeWindow>
  <Window fileName="User.cs" language="csharp">
  {`using EasyValidate;
using System;

public partial class User
{
    [NotEmpty(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
    public string Name { get; set; }
    
    [NotEmpty(ErrorMessage = "Email is required")]
    [Email(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }
}`}
  </Window>
  
  <Window fileName="Program.cs" language="csharp">
  {`using System;

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
}`}
  </Window>
  
  <Window fileName="Output" language="bash">
  {`Name: Name must be at least 3 characters
Email: Please enter a valid email`}
  </Window>
</CodeWindow>

## Configuration Example

<CodeWindow>
  <Window fileName="appsettings.json" language="json">
  {`{
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
}`}
  </Window>
  
  <Window fileName="Startup.cs" language="csharp">
  {`public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEasyValidate(options => {
            options.EnableLocalization = true;
            options.DefaultCulture = "en-US";
        });
        
        // Other service configurations
    }
}`}
  </Window>
</CodeWindow>

</DocSection>
