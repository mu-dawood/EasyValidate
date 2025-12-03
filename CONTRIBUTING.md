# Contributing to EasyValidate

Thank you for your interest in contributing to **EasyValidate** — a .NET validation library powered by Roslyn Source Generators.

We welcome contributions of all types: bug fixes, improvements, new validation rules, performance enhancements, and documentation updates.

---

## 🔧 Requirements

Before contributing, make sure you have:

* **.NET SDK 8.0+**
* **Visual Studio 2022** or **Rider** or **VS Code with C# Dev Kit**
* Basic familiarity with Roslyn analyzers & source generators (optional but recommended)

---

## 🚀 Getting Started

### 1. Fork the Repo

```bash
git clone https://github.com/mu-dawood/EasyValidate.git
cd EasyValidate
```

### 2. Open the Solution

Open:

```
EasyValidate.sln
```

The solution contains:

* **EasyValidate** — runtime library
* **EasyValidate.Generator** — Roslyn Source Generator
* **EasyValidate.Tests** — unit tests

---

## 🧪 Running Tests

We use **xUnit** for testing.

Run all tests:

```bash
dotnet test
```

Please ensure all tests pass before submitting a PR.

If you add new features or validation rules, include associated tests.

---

## 📦 Building the Source Generator

Roslyn Source Generators require a rebuild to see changes take effect.

To test generator behavior:

```bash
dotnet build
```

If you're testing generation output, consider enabling:

```xml
<EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
<CompilerGeneratedFilesOutputPath>Generated</CompilerGeneratedFilesOutputPath>
```

in your test project.

---

## 📝 Submitting Issues

Found a bug or have a feature request?

Open an issue here:
➡️ **[https://github.com/mu-dawood/EasyValidate/issues](https://github.com/mu-dawood/EasyValidate/issues)**

When reporting issues, include:

* Expected vs actual behavior
* Steps to reproduce
* Example model/class triggering the issue
* Generated code (if relevant)
* .NET version
* IDE (VS, Rider, VSCode)

---

## 🔧 Coding Standards

To keep the project consistent:

* Use C# 12 features when appropriate
* Follow existing folder structure
* Use `ILogger` when adding diagnostic logs
* For new validations:

  * Add logic in the generator
  * Add runtime validation (if required)
  * Add diagnostic messages
  * Add XML docs

Run formatting:

```bash
dotnet format
```

---

## 📚 Documentation

If your PR changes:

* Public API
* Attributes
* Validation flows
* Generator behavior

Then update:

* `README.md`
* Samples
* Wiki (if applicable)

---

## ➡️ Submitting Pull Requests

When ready:

1. Commit your changes with a descriptive message
2. Push your branch
3. Open a Pull Request to the **main** branch

PRs should include:

* Summary of changes
* Linked issue (if applicable)
* Screenshots or generated code examples (optional but helpful)

We aim to review PRs within **3–5 business days**.

---

## 🤝 Code of Conduct

By contributing, you agree to follow our project’s Code of Conduct.
