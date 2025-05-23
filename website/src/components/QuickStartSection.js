import React, { useState, useRef } from 'react';
import clsx from 'clsx';
import CodeWindow from './CodeWindow';
import InlineSnippet from './InlineSnippet';
import styles from './QuickStartSection.module.css';

function QuickStartSection() {
  const [activeStep, setActiveStep] = useState(0);

  const steps = [
    {
      title: "Install Package",
      description: "Add EasyValidate to your .NET project using NuGet Package Manager",
      fileName: "Package Manager Console",
      code: `dotnet add package EasyValidate

# Or using Package Manager in Visual Studio
Install-Package EasyValidate

# Or using PackageReference in .csproj
<PackageReference Include="EasyValidate" Version="1.0.0" />`,
      language: "bash",
      icon: "📦"
    },
    {
      title: "Define Your Model",
      description: "Create your data model with validation attributes",
      fileName: "Models/UserModel.cs",
      code: `using EasyValidate.Attributes;

public partial class User
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [Email]
    public string Email { get; set; }
    
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }
}`,
      language: "csharp",
      icon: "🏗️"
    },
    {
      title: "Use Validation",
      description: "Call the auto-generated validation method and handle results",
      fileName: "Services/UserService.cs",
      code: `using EasyValidate.Abstractions;

public class UserService
{
    public async Task<bool> CreateUserAsync(User user)
    {
        // The Validate() method is auto-generated at compile-time
        ValidationResult result = user.Validate();

        if (!result.IsValid)
        {
            Console.WriteLine($"Validation failed with {result.Errors.Count} errors:");
            
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"• {error.PropertyName}: {error.ErrorMessage}");
            }
            
            return false;
        }

        // Process valid user...
        await SaveUserToDatabase(user);
        return true;
    }
}

// Example output for invalid data:
// Validation failed with 3 errors:
// • Name: The field Name must be a string with a minimum length of 2 and a maximum length of 50.
// • Email: The Email field is not a valid e-mail address.
// • PhoneNumber: The PhoneNumber field is not a valid phone number.`,
      language: "csharp",
      icon: "✅"
    }
  ];

  const currentStep = steps[activeStep];

  return (
    <section className={styles.quickStartSection}>
      <div className={styles.container}>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>Quick Start Guide</h2>
          <p className={styles.sectionSubtitle}>
            Get up and running with EasyValidate in just 3 simple steps
          </p>
        </div>
        
        <div className={styles.quickStartGrid}>
          {/* Steps Navigation */}
          <div className={styles.stepsNavigation}>
            <div className={styles.stepsHeader}>
              <h3 className={styles.stepsTitle}>Setup Steps</h3>
              <div className={styles.progressIndicator}>
                <span className={styles.currentStepNum}>{activeStep + 1}</span>
                <span className={styles.stepSeparator}>of</span>
                <span className={styles.totalStepsNum}>{steps.length}</span>
              </div>
            </div>

            <div className={styles.stepsList}>
              {steps.map((step, index) => (
                <div 
                  key={index}
                  className={clsx(
                    styles.stepItem,
                    { [styles.active]: activeStep === index },
                    { [styles.completed]: activeStep > index }
                  )}
                  onClick={() => setActiveStep(index)}
                >
                  <div className={styles.stepIcon}>
                    {activeStep > index ? '✓' : step.icon}
                  </div>
                  <div className={styles.stepContent}>
                    <h4 className={styles.stepTitle}>{step.title}</h4>
                    <p className={styles.stepDescription}>{step.description}</p>
                  </div>
                  <div className={styles.stepConnector}></div>
                </div>
              ))}
            </div>

            <div className={styles.navigationButtons}>
              <button 
                className={clsx(styles.navBtn, styles.prevBtn)}
                onClick={() => setActiveStep(Math.max(0, activeStep - 1))}
                disabled={activeStep === 0}
              >
                ← Previous Step
              </button>
              <button 
                className={clsx(styles.navBtn, styles.nextBtn)}
                onClick={() => setActiveStep(Math.min(steps.length - 1, activeStep + 1))}
                disabled={activeStep === steps.length - 1}
              >
                Next Step →
              </button>
            </div>
          </div>

          {/* Code Display */}
          <div className={styles.codeDisplay}>
            <CodeWindow 
              fileName={currentStep.fileName}
              language={currentStep.language}
              variant="light"
              showCopyButton={true}
            >
              {currentStep.code}
            </CodeWindow>
            
            <div className={styles.stepInfo}>
              <div className={styles.stepBadge}>
                Step {activeStep + 1} of {steps.length}
              </div>
              <h3 className={styles.stepInfoTitle}>{currentStep.title}</h3>
              <p className={styles.stepInfoDescription}>{currentStep.description}</p>
              
              <div className={styles.stepTips}>
                {activeStep === 0 && (
                  <div className={styles.tip}>
                    <span className={styles.tipIcon}>💡</span>
                    <span>EasyValidate uses source generators to create validation methods at compile-time for maximum performance.</span>
                  </div>
                )}
                {activeStep === 1 && (
                  <div className={styles.tip}>
                    <span className={styles.tipIcon}>💡</span>
                    <span>The <code>partial</code> keyword is required for the source generator to add the validation method to your class.</span>
                  </div>
                )}
                {activeStep === 2 && (
                  <div className={styles.tip}>
                    <span className={styles.tipIcon}>💡</span>
                    <span>The <code>Validate()</code> method is automatically generated and returns detailed error information for failed validations.</span>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default QuickStartSection;
