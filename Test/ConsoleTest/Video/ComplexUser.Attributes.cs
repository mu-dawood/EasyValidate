using EasyValidate.Abstractions;
using EasyValidate.Attributes;

namespace ConsoleTest.Video;

public partial class ComplexUserWithAttributes : IGenerate
{
    [NotEmpty]
    public string Name { get; set; } = string.Empty;

    [NotNull]
    public User? UserDetails { get; set; }

    [HasElements]
    public List<User> Friends { get; set; } = [];

    [Range(0, 120)]
    public int Age { get; set; }

    [NotEmpty]
    public string Address { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [PastDate]
    public DateTime DateOfBirth { get; set; }

    [EqualTo(true)]
    public bool IsActive { get; set; }

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [HasElements]
    public List<string> Tags { get; set; } = [];

    [Range(0.0, 5.0)]
    public double Rating { get; set; }

    [NotDefault()]
    public Guid Id { get; set; } = Guid.NewGuid();
}