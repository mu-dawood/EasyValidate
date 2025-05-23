---
id: quickstart
title: Quick Start
---

# Quick Start

```csharp
public class UserDto
{
    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }

    [Range(18, 99)]
    public int Age { get; set; }

    [MaxLength(20)]
    public string Username { get; set; }
}
```

See [Attributes Reference](attributes.md) for all available validation attributes.
