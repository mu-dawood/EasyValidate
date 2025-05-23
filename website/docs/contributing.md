---
id: contributing
title: Contributing
sidebar_position: 7
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid } from '@site/src/components/DocsComponents';

<DocsWrapper>

# Contributing to EasyValidate

We welcome contributions from the community! This guide will help you get started with contributing to EasyValidate, whether you're fixing bugs, adding features, improving documentation, or helping with testing.

## 🚀 Getting Started

### Prerequisites

Before contributing, ensure you have:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later
- [Git](https://git-scm.com/) for version control
- A code editor (Visual Studio, VS Code, or Rider recommended)
- [Node.js](https://nodejs.org/) (for documentation website)

### Development Environment Setup

1. **Fork and Clone the Repository**
   ```bash
   # Fork the repository on GitHub, then clone your fork
   git clone https://github.com/your-username/EasyValidate.git
   cd EasyValidate
   
   # Add the upstream remote
   git remote add upstream https://github.com/original-org/EasyValidate.git
   ```

2. **Install Dependencies**
   ```bash
   # Restore .NET packages
   dotnet restore
   
   # Install documentation dependencies (if working on docs)
   cd website
   npm install
   cd ..
   ```

3. **Verify Setup**
   ```bash
   # Build the solution
   dotnet build
   
   # Run tests
   dotnet test
   ```

## 🏗️ Project Structure

Understanding the project structure will help you navigate and contribute effectively:

```
EasyValidate/
├── EasyValidate/                 # Core library
│   ├── Abstraction/             # Base classes and interfaces
│   ├── Attributes/              # Validation attributes
│   ├── Formatters/              # Error message formatters
│   └── Validation/              # Core validation logic
├── EasyValidate.Analyzers/      # Source generators and analyzers
├── EasyValidate.Tests/          # Unit and integration tests
├── samples/                     # Example applications
├── website/                     # Documentation website
└── docs/                       # Additional documentation
```

## 🐛 Reporting Issues

### Before Creating an Issue

1. **Search existing issues** to avoid duplicates
2. **Check the documentation** to ensure it's not a usage question
3. **Test with the latest version** to see if the issue still exists

### Creating a Good Issue

When reporting bugs or requesting features, please include:

**For Bug Reports:**
- **Clear title** describing the issue
- **Steps to reproduce** the problem
- **Expected behavior** vs actual behavior
- **Code samples** demonstrating the issue
- **Environment details** (.NET version, OS, etc.)
- **Stack traces** if applicable

**For Feature Requests:**
- **Use case description** explaining why the feature is needed
- **Proposed API** or behavior
- **Alternative solutions** you've considered
- **Examples** of how it would be used

### Issue Template

```markdown
## Description
Brief description of the issue

## Steps to Reproduce
1. Step one
2. Step two
3. Step three

## Expected Behavior
What you expected to happen

## Actual Behavior
What actually happened

## Code Sample
```csharp
// Minimal code that reproduces the issue
```

## Environment
- EasyValidate Version: x.x.x
- .NET Version: x.x.x
- OS: Windows/macOS/Linux
```

## 💻 Making Changes

### Workflow

1. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   # or
   git checkout -b fix/issue-number-description
   ```

2. **Make Your Changes**
   - Follow our [coding standards](#coding-standards)
   - Add tests for new functionality
   - Update documentation as needed

3. **Test Your Changes**
   ```bash
   # Run all tests
   dotnet test
   
   # Run specific test project
   dotnet test EasyValidate.Tests
   
   # Check code coverage (if configured)
   dotnet test --collect:"XPlat Code Coverage"
   ```

4. **Commit Your Changes**
   ```bash
   git add .
   git commit -m "feat: add new validation attribute for credit cards"
   ```

5. **Push and Create Pull Request**
   ```bash
   git push origin feature/your-feature-name
   ```

### Coding Standards

#### General Guidelines

- **Follow .NET naming conventions** (PascalCase for public members, camelCase for private)
- **Use meaningful names** for variables, methods, and classes
- **Keep methods focused** and single-responsibility
- **Add XML documentation** for public APIs
- **Include unit tests** for all new functionality

#### Code Style

```csharp
// ✅ Good: Clear, documented, tested
/// <summary>
/// Validates that a string contains only alphabetic characters.
/// </summary>
/// <param name="allowSpaces">Whether to allow space characters.</param>
[AttributeUsage(AttributeTargets.Property)]
public class AlphaAttribute : StringValidationAttribute
{
    public bool AllowSpaces { get; set; }

    public AlphaAttribute(bool allowSpaces = false)
    {
        AllowSpaces = allowSpaces;
    }

    protected override bool IsValid(string value)
    {
        if (string.IsNullOrEmpty(value))
            return true;

        return AllowSpaces 
            ? value.All(c => char.IsLetter(c) || char.IsWhiteSpace(c))
            : value.All(char.IsLetter);
    }

    protected override string GetDefaultErrorMessage()
    {
        return AllowSpaces 
            ? "The field {0} must contain only letters and spaces."
            : "The field {0} must contain only letters.";
    }
}
```

#### Testing Standards

- **Use descriptive test names** that explain the scenario
- **Follow Arrange-Act-Assert** pattern
- **Test edge cases** and error conditions
- **Use theory tests** for multiple input scenarios

```csharp
[Theory]
[InlineData("ValidEmail@example.com", true)]
[InlineData("invalid.email", false)]
[InlineData("", true)] // Empty is valid (use Required for mandatory)
[InlineData(null, true)]
public void EmailAttribute_ValidatesCorrectly(string email, bool expected)
{
    // Arrange
    var attribute = new EmailAttribute();
    var context = new ValidationContext(new { Email = email });

    // Act
    var result = attribute.GetValidationResult(email, context);

    // Assert
    Assert.Equal(expected, result == ValidationResult.Success);
}
```

## 🧪 Testing Guidelines

### Test Organization

- **Unit Tests**: Test individual classes and methods in isolation
- **Integration Tests**: Test component interactions
- **Performance Tests**: Verify performance characteristics
- **Analyzer Tests**: Test source generators and analyzers

### Writing Good Tests

1. **Test behavior, not implementation**
2. **Use meaningful test data**
3. **Test error conditions**
4. **Keep tests focused and fast**
5. **Use parameterized tests** for similar scenarios

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "ClassName=EmailAttributeTests"

# Run tests matching pattern
dotnet test --filter "Name~Email"
```

## 📝 Documentation Contributions

### Documentation Types

1. **API Documentation**: XML comments in code
2. **User Guides**: Markdown files in `website/docs/`
3. **Examples**: Sample applications in `samples/`
4. **README**: Project overview and quick start

### Documentation Standards

- **Write for your audience**: Beginners to advanced users
- **Use clear examples**: Show real-world usage
- **Keep it current**: Update when APIs change
- **Test examples**: Ensure code samples work

### Building Documentation Locally

```bash
cd website
npm start
```

The documentation site will be available at `http://localhost:3000`.

## 🔄 Pull Request Process

### Before Submitting

1. **Sync with upstream**
   ```bash
   git fetch upstream
   git rebase upstream/main
   ```

2. **Run the full test suite**
   ```bash
   dotnet test
   ```

3. **Check for merge conflicts**
4. **Update documentation** if needed

### Pull Request Guidelines

- **Use descriptive titles** following conventional commits
- **Reference related issues** (e.g., "Fixes #123")
- **Describe your changes** in the PR description
- **Include test results** if relevant
- **Keep PRs focused** - one feature or fix per PR

### PR Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix (non-breaking change)
- [ ] New feature (non-breaking change)
- [ ] Breaking change (fix or feature that breaks existing functionality)
- [ ] Documentation update

## Testing
- [ ] Added unit tests
- [ ] Added integration tests
- [ ] Existing tests pass
- [ ] Manual testing completed

## Checklist
- [ ] Code follows project style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No new compiler warnings
```

## 🎯 Contribution Areas

### Code Contributions

**New Validation Attributes**
- Email formats, phone numbers, credit cards
- Regional-specific validations
- Business rule validations

**Performance Improvements**
- Optimization of existing validators
- Memory usage improvements
- Analyzer enhancements

**Platform Support**
- Additional .NET framework versions
- Platform-specific optimizations

### Documentation Contributions

- **Tutorial improvements**
- **Example applications**
- **Translation** to other languages
- **Video tutorials** or blog posts

### Community Contributions

- **Answer questions** in issues and discussions
- **Review pull requests**
- **Share usage examples**
- **Blog about EasyValidate**

## 🏷️ Commit Message Convention

We follow [Conventional Commits](https://www.conventionalcommits.org/):

- `feat:` New features
- `fix:` Bug fixes
- `docs:` Documentation changes
- `style:` Code style changes (formatting, etc.)
- `refactor:` Code refactoring
- `test:` Adding or updating tests
- `chore:` Maintenance tasks

Examples:
```
feat: add CreditCardAttribute for payment validation
fix: resolve null reference in EmailAttribute
docs: update installation guide with .NET 8 support
test: add comprehensive tests for PhoneAttribute
```

## 🚀 Release Process

### Versioning

We follow [Semantic Versioning](https://semver.org/):
- **Major** (X.0.0): Breaking changes
- **Minor** (0.X.0): New features (backward compatible)
- **Patch** (0.0.X): Bug fixes (backward compatible)

### Release Workflow

1. **Version bump** in project files
2. **Update changelog** with new features and fixes
3. **Create release branch**
4. **Run full test suite**
5. **Create GitHub release**
6. **Publish NuGet packages**

## ❓ Getting Help

### Community Support

- **GitHub Discussions**: Ask questions and share ideas
- **GitHub Issues**: Report bugs and request features
- **Stack Overflow**: Tag questions with `easyvalidate`

### Maintainer Contact

For urgent issues or security concerns, contact the maintainers directly through GitHub.

## 🙏 Recognition

Contributors are recognized in:
- **Release notes** for significant contributions
- **Contributors section** in README
- **GitHub contributors** page

Thank you for contributing to EasyValidate! Your efforts help make .NET validation easier for everyone.

## 📚 Additional Resources

- [.NET Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [xUnit Documentation](https://xunit.net/)
- [GitHub Flow](https://guides.github.com/introduction/flow/)
- [Semantic Versioning](https://semver.org/)
</DocsWrapper>
---

© 2025 EasyValidate Authors. MIT License.
