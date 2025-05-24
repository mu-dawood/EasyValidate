import CodeWindow, { type Window } from '../CodeWindow';
import InlineSnippet from '../InlineSnippet';
import styles from './GettingStarted.module.css';

function GettingStarted() {
  const installationSteps: Window[] = [
    {
      fileName: 'Package Manager Console',
      language: 'powershell',
      snipt: 'Install-Package EasyValidate',
      active: true
    },
    {
      fileName: '.NET CLI',
      language: 'bash',
      snipt: 'dotnet add package EasyValidate'
    },
    {
      fileName: 'PackageReference',
      language: 'xml',
      snipt: `<PackageReference Include="EasyValidate" Version="1.0.0" />`
    }
  ];

  const quickStartCode: Window[] = [
    {
      fileName: 'UserModel.cs',
      language: 'csharp',
      snipt: `using EasyValidate.Attributes;

public partial class User
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [Email]
    public string Email { get; set; }

    [Range(18, 120)]
    public int Age { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}`,
      active: true
    },
    {
      fileName: 'Program.cs',
      language: 'csharp',
      snipt: `var user = new User
{
    Name = "John Doe",
    Email = "john@example.com",
    Age = 25,
    PhoneNumber = "+1-555-123-4567"
};

var result = user.Validate();

if (result.IsValid)
{
    Console.WriteLine("User is valid!");
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.Property}: {error.Message}");
    }
}`
    }
  ];

  return (
    <section id="getting-started" className={styles.gettingStarted}>
      <div className={styles.container}>
        <div className={styles.header}>
          <h2 className={styles.title}>Getting Started</h2>
          <p className={styles.subtitle}>
            Get up and running with EasyValidate in minutes, not hours.
          </p>
        </div>

        <div className={styles.steps}>
          <div className={styles.step}>
            <div className={styles.stepNumber}>1</div>
            <div className={styles.stepContent}>
              <h3 className={styles.stepTitle}>Install the Package</h3>
              <p className={styles.stepDescription}>
                Add EasyValidate to your project using your preferred package manager.
              </p>
              <div className={styles.codeBlock}>
                <CodeWindow windows={installationSteps} variant="light" />
              </div>
            </div>
          </div>

          <div className={styles.step}>
            <div className={styles.stepNumber}>2</div>
            <div className={styles.stepContent}>
              <h3 className={styles.stepTitle}>Add Validation Attributes</h3>
              <p className={styles.stepDescription}>
                Decorate your model properties with validation attributes. Make your class <InlineSnippet language="csharp">partial</InlineSnippet> to enable source generation.
              </p>
              <div className={styles.highlight}>
                <strong>💡 Pro Tip:</strong> The <code>partial</code> keyword is required for source generation to work properly.
              </div>
            </div>
          </div>

          <div className={styles.step}>
            <div className={styles.stepNumber}>3</div>
            <div className={styles.stepContent}>
              <h3 className={styles.stepTitle}>Validate Your Models</h3>
              <p className={styles.stepDescription}>
                Call the generated <InlineSnippet language="csharp">Validate()</InlineSnippet> method to get comprehensive validation results.
              </p>
              <div className={styles.codeBlock}>
                <CodeWindow windows={quickStartCode} variant="light" />
              </div>
            </div>
          </div>
        </div>

        <div className={styles.nextSteps}>
          <h3 className={styles.nextStepsTitle}>What's Next?</h3>
          <div className={styles.nextStepsGrid}>
            <a href="/docs/examples" className={styles.nextStepCard}>
              <span className={styles.nextStepIcon}>📚</span>
              <span className={styles.nextStepText}>Explore Examples</span>
            </a>
            <a href="/docs/intro" className={styles.nextStepCard}>
              <span className={styles.nextStepIcon}>📖</span>
              <span className={styles.nextStepText}>Read Documentation</span>
            </a>
            <a href="https://github.com/mu-dawood/EasyValidate" className={styles.nextStepCard}>
              <span className={styles.nextStepIcon}>⭐</span>
              <span className={styles.nextStepText}>Star on GitHub</span>
            </a>
          </div>
        </div>
      </div>
    </section>
  );
}

export default GettingStarted;
