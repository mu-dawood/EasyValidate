<p align="center">
  <a href="https://coff.ee/mu.dawood" target="_blank">
    <img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" height="60" width="217" />
  </a>
  <br/>
  <b>If you find EasyValidate helpful, consider <a href="https://coff.ee/mu.dawood">buying me a coffee</a> to support development!</b>
</p>

# EasyValidate

A modern, type-safe .NET validation library and source generator with Roslyn analyzers and code fixers.

## Overview

EasyValidate enables powerful, composable, and type-safe validation using attributes. It generates efficient validation code at compile time and provides analyzers for compile-time feedback.

## Quick Start

```csharp
public class User
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [Email]
    public string Email { get; set; }
    
    [Range(18, 120)]
    public int Age { get; set; }
}

// Generated validation method
var result = user.Validate();
if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error.Message);
    }
}
```

## Installation

Install the NuGet package:

```bash
dotnet add package EasyValidate
```

## Documentation

📖 **Full documentation:** [https://easy-validate.netlify.app](https://easy-validate.netlify.app)

- Getting started
- Complete API reference
- Attribute documentation
- Migration guides
- Examples and tutorials

## Project Structure

- `EasyValidate.Core/` – Core validation attributes and logic (type-safe, scalable)
- `EasyValidate/` – Source generator and NuGet package entry point
- `EasyValidate.Analyzers/` – Roslyn analyzers for compile-time validation
- `EasyValidate.Fixers/` – Roslyn code fixers for quick fixes
- `EasyValidate.Test/` – Unit tests
- `ConsoleTest/` – Console app for manual testing
- `docs/` – Additional docs and assets


## License

This project is licensed under the GPL-3.0-only License – see the [LICENSE](LICENSE) file for details.

## Support

- 🐛 **Issues:** [GitHub Issues](https://github.com/mu-dawood/EasyValidate/issues)
- 📖 **Documentation:** [Documentation Website](https://easy-validate.netlify.app)
- 💬 **Discussions:** [GitHub Discussions](https://github.com/mu-dawood/EasyValidate/discussions)
- ☕️ **Buy me a coffee:** [https://www.buymeacoffee.com/mudawood](https://www.buymeacoffee.com/mudawood)
