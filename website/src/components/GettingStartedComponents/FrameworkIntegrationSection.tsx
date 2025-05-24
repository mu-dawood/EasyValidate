import { useState } from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './GettingStartedComponents.module.css';

interface Framework {
  id: string;
  name: string;
  icon: string;
  description: string;
  setupCode: string;
  usageCode: string;
  features: string[];
}

function FrameworkIntegrationSection() {
  const [activeFramework, setActiveFramework] = useState('aspnet-core');
  const [activeTab, setActiveTab] = useState<'setup' | 'usage'>('setup');

  const frameworks: Framework[] = [
    {
      id: 'aspnet-core',
      name: 'ASP.NET Core',
      icon: '🌐',
      description: 'Web APIs and MVC applications',
      features: ['Automatic model validation', 'Built-in error responses', 'Dependency injection', 'Middleware support'],
      setupCode: `// Program.cs or Startup.cs
using EasyValidate;
using EasyValidate.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add EasyValidate services
builder.Services.AddEasyValidate(options =>
{
    options.EnableDetailedErrors = true;
    options.StopOnFirstFailure = false;
    options.CascadeMode = CascadeMode.Continue;
});

// Add controllers with validation
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

var app = builder.Build();

// Use validation middleware
app.UseEasyValidate();
app.MapControllers();

app.Run();`,
      usageCode: `[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        // Validation is automatic with EasyValidate middleware
        // If validation fails, BadRequest is returned automatically
        
        // Your business logic here
        var userId = await userService.CreateUserAsync(user);
        
        return Ok(new { Id = userId, Message = "User created successfully" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var user = await userService.GetUserAsync(id);
        if (user == null) return NotFound();

        // Manual validation when needed
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await userService.UpdateUserAsync(id, request);
        return NoContent();
    }
}`
    },
    {
      id: 'blazor',
      name: 'Blazor',
      icon: '⚡',
      description: 'Interactive web UIs with C#',
      features: ['Component validation', 'Real-time feedback', 'Form integration', 'Client-side validation'],
      setupCode: `// Program.cs (Blazor Server or WebAssembly)
using EasyValidate;
using EasyValidate.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add Blazor services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add EasyValidate with Blazor support
builder.Services.AddEasyValidate();
builder.Services.AddEasyValidateBlazor();

var app = builder.Build();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();`,
      usageCode: `@page "/user-form"
@using EasyValidate.Blazor
@inject IJSRuntime JSRuntime

<h3>Create User</h3>

<EditForm Model="@user" OnValidSubmit="@HandleValidSubmit">
    <EasyValidateValidator />
    
    <div class="form-group">
        <label for="name">Name:</label>
        <InputText id="name" @bind-Value="user.Name" class="form-control" />
        <ValidationMessage For="@(() => user.Name)" />
    </div>

    <div class="form-group">
        <label for="email">Email:</label>
        <InputText id="email" @bind-Value="user.Email" class="form-control" />
        <ValidationMessage For="@(() => user.Email)" />
    </div>

    <div class="form-group">
        <label for="age">Age:</label>
        <InputNumber id="age" @bind-Value="user.Age" class="form-control" />
        <ValidationMessage For="@(() => user.Age)" />
    </div>

    <button type="submit" class="btn btn-primary" disabled="@(!IsFormValid)">
        Create User
    </button>
</EditForm>

@code {
    private User user = new();
    private bool IsFormValid => user.Validate().IsValid;

    private async Task HandleValidSubmit()
    {
        await JSRuntime.InvokeVoidAsync("alert", $"User {user.Name} created successfully!");
        user = new User(); // Reset form
    }
}`
    },
    {
      id: 'wpf',
      name: 'WPF',
      icon: '🖥️',
      description: 'Desktop applications with XAML',
      features: ['MVVM pattern support', 'Data binding validation', 'Custom error templates', 'Real-time validation'],
      setupCode: `// App.xaml.cs
using EasyValidate;
using EasyValidate.Wpf;
using Microsoft.Extensions.DependencyInjection;

public partial class App : Application
{
    public IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        
        // Configure EasyValidate
        services.AddEasyValidate();
        services.AddEasyValidateWpf();
        
        // Register ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<UserViewModel>();
        
        ServiceProvider = services.BuildServiceProvider();
        
        var mainWindow = new MainWindow
        {
            DataContext = ServiceProvider.GetRequiredService<MainViewModel>()
        };
        
        mainWindow.Show();
        base.OnStartup(e);
    }
}`,
      usageCode: `// UserViewModel.cs
using EasyValidate.Wpf;
using System.ComponentModel;

public class UserViewModel : ValidatableViewModel
{
    private string _name = string.Empty;
    private string _email = string.Empty;
    private int _age;

    [Required]
    [StringLength(50)]
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    [Required]
    [EmailAddress]
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    [Range(18, 100)]
    public int Age
    {
        get => _age;
        set => SetProperty(ref _age, value);
    }

    public ICommand SaveCommand => new RelayCommand(
        execute: () => Save(),
        canExecute: () => !HasErrors
    );

    private void Save()
    {
        if (Validate())
        {
            // Save user logic
            MessageBox.Show($"User {Name} saved successfully!");
        }
    }
}`
    },
    {
      id: 'maui',
      name: '.NET MAUI',
      icon: '📱',
      description: 'Cross-platform mobile and desktop apps',
      features: ['Multi-platform support', 'Native UI validation', 'Cross-platform validation', 'Shared business logic'],
      setupCode: `// MauiProgram.cs
using EasyValidate;
using EasyValidate.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Add EasyValidate services
        builder.Services.AddEasyValidate();
        builder.Services.AddEasyValidateMaui();
        
        // Register ViewModels
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<UserFormViewModel>();

        return builder.Build();
    }
}`,
      usageCode: `// UserFormPage.xaml.cs
using EasyValidate.Maui;

public partial class UserFormPage : ContentPage
{
    public UserFormPage(UserFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

// UserFormViewModel.cs
public class UserFormViewModel : ValidatableViewModel
{
    private User _user = new();

    public User User
    {
        get => _user;
        set => SetProperty(ref _user, value);
    }

    public Command SaveCommand => new Command(
        execute: async () => await SaveUser(),
        canExecute: () => User.Validate().IsValid
    );

    private async Task SaveUser()
    {
        var result = User.Validate();
        if (result.IsValid)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Success", 
                $"User {User.Name} saved!", 
                "OK"
            );
        }
        else
        {
            var errors = string.Join("\\n", result.Errors.Select(e => e.Message));
            await Application.Current.MainPage.DisplayAlert(
                "Validation Error", 
                errors, 
                "OK"
            );
        }
    }
}`
    }
  ];

  const activeFrameworkData = frameworks.find(f => f.id === activeFramework) || frameworks[0];
  const activeCode = activeTab === 'setup' ? activeFrameworkData.setupCode : activeFrameworkData.usageCode;

  return (
    <section className={styles.section}>
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>
          <span className={styles.sectionIcon}>🚀</span>
          Framework Integration
        </h2>
        <p className={styles.sectionDescription}>
          EasyValidate works seamlessly with all popular .NET frameworks and platforms
        </p>
      </div>

      <div className={styles.frameworkContainer}>
        {/* Framework Selection */}
        <div className={styles.frameworkTabs}>
          {frameworks.map((framework) => (
            <button
              key={framework.id}
              className={`${styles.frameworkTab} ${activeFramework === framework.id ? styles.active : ''}`}
              onClick={() => setActiveFramework(framework.id)}
            >
              <span className={styles.frameworkIcon}>{framework.icon}</span>
              <div className={styles.frameworkInfo}>
                <span className={styles.frameworkName}>{framework.name}</span>
                <span className={styles.frameworkDescription}>{framework.description}</span>
              </div>
            </button>
          ))}
        </div>

        {/* Framework Details */}
        <div className={styles.frameworkDetails}>
          <div className={styles.frameworkHeader}>
            <div className={styles.frameworkTitle}>
              <span className={styles.frameworkIcon}>{activeFrameworkData.icon}</span>
              <h3>{activeFrameworkData.name}</h3>
            </div>
            <p className={styles.frameworkDescription}>{activeFrameworkData.description}</p>
          </div>

          {/* Features */}
          <div className={styles.featuresGrid}>
            {activeFrameworkData.features.map((feature, index) => (
              <div key={index} className={styles.featureItem}>
                <span className={styles.featureIcon}>✅</span>
                <span className={styles.featureText}>{feature}</span>
              </div>
            ))}
          </div>

          {/* Code Tabs */}
          <div className={styles.codeTabs}>
            <button
              className={`${styles.codeTab} ${activeTab === 'setup' ? styles.active : ''}`}
              onClick={() => setActiveTab('setup')}
            >
              🔧 Setup
            </button>
            <button
              className={`${styles.codeTab} ${activeTab === 'usage' ? styles.active : ''}`}
              onClick={() => setActiveTab('usage')}
            >
              💻 Usage
            </button>
          </div>

          {/* Code Example */}
          <div className={styles.codeContainer}>
            <div className={styles.codeHeader}>
              <h4>{activeTab === 'setup' ? 'Project Setup' : 'Usage Example'}</h4>
              <button 
                className={styles.copyButton}
                onClick={() => navigator.clipboard.writeText(activeCode)}
                title="Copy code"
              >
                📋
              </button>
            </div>
            
            <div className={styles.codeContent}>
              <InlineSnippet 
                snipt={activeCode}
                language="csharp"
                className={styles.frameworkCode}
              />
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default FrameworkIntegrationSection;
