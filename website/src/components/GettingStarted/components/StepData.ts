// Step data and types for the Getting Started stepper
export interface Window {
  fileName: string;
  language: string;
  snipt: string;
  active?: boolean;
}

export interface StepContent {
  type: string;
  windows: Window[];
  tip?: string;
}

export interface Step {
  id: number;
  title: string;
  description: string;
  icon: string;
  content: StepContent;
}

export const steps: Step[] = [
  {
    id: 0,
    title: "Install EasyValidate",
    description: "Add the NuGet package to your .NET project",
    icon: "📦",
    content: {
      type: "installation",
      windows: [
        {
          fileName: '.NET CLI',
          language: 'bash',
          snipt: 'dotnet add package EasyValidate',
          active: true
        },
        {
          fileName: 'Package Manager Console',
          language: 'powershell',
          snipt: 'Install-Package EasyValidate'
        },
        {
          fileName: 'PackageReference',
          language: 'xml',
          snipt: `<PackageReference Include="EasyValidate" Version="1.0.0" />`
        }
      ]
    }
  },
  {
    id: 1,
    title: "Add Validation Attributes",
    description: "Decorate your models with attributes. The partial keyword enables source generation.",
    icon: "✨",
    content: {
      type: "code",
      windows: [
        {
          fileName: 'UserModel.cs',
          language: 'csharp',
          snipt: `using EasyValidate.Attributes;

public partial class User
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2, 
        ErrorMessage = "Name must be 2-50 characters")]
    public string Name { get; set; }

    [Required]
    [Email(ErrorMessage = "Please enter a valid email")]
    public string Email { get; set; }

    [Range(18, 120, ErrorMessage = "Age must be between 18-120")]
    public int Age { get; set; }
}`,
          active: true
        },
        {
          fileName: 'Program.cs',
          language: 'csharp',
          snipt: `// Create and validate a user
var user = new User
{
    Name = "John Doe",
    Email = "john@example.com",
    Age = 25
};

// Get validation results
var result = user.Validate();

if (result.IsValid)
{
    Console.WriteLine("✅ User is valid!");
}
else
{
    Console.WriteLine("❌ Validation failed:");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"  {error.Property}: {error.Message}");
    }
}`
        },
        {
          fileName: 'Output',
          language: 'text',
          snipt: `✅ User is valid!`
        }
      ],
      tip: "The partial keyword is required for source generation to work"
    }
  },
  {
    id: 2,
    title: "Explore Advanced Features",
    description: "Nested validation, collections, custom attributes, and more",
    icon: "🎯",
    content: {
      type: "code",
      windows: [
        {
          fileName: 'ProductModel.cs',
          language: 'csharp',
          snipt: `public partial class Product
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL")]
    public string Website { get; set; }

    // Nested validation
    [Required]
    public Category Category { get; set; }

    // Collection validation
    [CollectionNotEmpty]
    [MaxLength(10)]
    public List<string> Tags { get; set; }
}

public partial class Category
{
    [Required, StringLength(50)]
    public string Name { get; set; }
}`,
          active: true
        },
        {
          fileName: 'Usage.cs',
          language: 'csharp',
          snipt: `var product = new Product
{
    Name = "Awesome Product",
    Price = 99.99m,
    Website = "https://example.com",
    Category = new Category { Name = "Electronics" },
    Tags = new List<string> { "tech", "gadget" }
};

var result = product.Validate();
// Automatically validates nested objects and collections!`
        }
      ]
    }
  }
];
