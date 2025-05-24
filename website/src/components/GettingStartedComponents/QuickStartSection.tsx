import { useState } from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './GettingStartedComponents.module.css';

interface Step {
  id: string;
  title: string;
  description: string;
  icon: string;
  code: string;
  language: string;
  highlights: string[];
}

function QuickStartSection() {
  const [activeStep, setActiveStep] = useState(0);

  const steps: Step[] = [
    {
      id: 'install',
      title: 'Install EasyValidate',
      description: 'Add the NuGet package to your project',
      icon: '📦',
      language: 'bash',
      highlights: ['Zero dependencies', 'Cross-platform', 'Latest stable version'],
      code: `# Install via Package Manager
dotnet add package EasyValidate

# Or via NuGet Package Manager
Install-Package EasyValidate`
    },
    {
      id: 'define-model',
      title: 'Create Your Model',
      description: 'Define a class with validation attributes',
      icon: '🏗️',
      language: 'csharp',
      highlights: ['Partial class required', 'Rich validation attributes', 'IntelliSense support'],
      code: `using EasyValidate.Attributes;

public partial class User
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(18, 120)]
    public int Age { get; set; }
}`
    },
    {
      id: 'validate',
      title: 'Validate Your Data',
      description: 'Use the auto-generated validation method',
      icon: '✅',
      language: 'csharp',
      highlights: ['Zero reflection', 'Compile-time generation', 'Type-safe results'],
      code: `var user = new User
{
    Name = "John Doe",
    Email = "john@example.com",
    Age = 25
};

// Auto-generated validation method
var result = user.Validate();

if (result.IsValid)
{
    Console.WriteLine("✅ User is valid!");
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"❌ {error.Property}: {error.Message}");
    }
}`
    }
  ];

  const currentStep = steps[activeStep];

  return (
    <section className={styles.section}>
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>
          <span className={styles.sectionIcon}>🚀</span>
          Quick Start
        </h2>
        <p className={styles.sectionDescription}>
          Get started with EasyValidate in just 3 simple steps
        </p>
      </div>

      <div className={styles.quickStartContainer}>
        {/* Step Progress */}
        <div className={styles.stepProgress}>
          {steps.map((step, index) => (
            <div
              key={step.id}
              className={`${styles.stepItem} ${activeStep === index ? styles.active : ''} ${activeStep > index ? styles.completed : ''}`}
              onClick={() => setActiveStep(index)}
            >
              <div className={styles.stepCircle}>
                {activeStep > index ? '✓' : step.icon}
              </div>
              <div className={styles.stepInfo}>
                <h3 className={styles.stepTitle}>{step.title}</h3>
                <p className={styles.stepDesc}>{step.description}</p>
              </div>
              {index < steps.length - 1 && <div className={styles.stepConnector} />}
            </div>
          ))}
        </div>

        {/* Current Step Content */}
        <div className={styles.stepContent}>
          <div className={styles.stepHeader}>
            <div className={styles.stepBadge}>
              Step {activeStep + 1} of {steps.length}
            </div>
            <h3 className={styles.currentStepTitle}>
              {currentStep.icon} {currentStep.title}
            </h3>
            <p className={styles.currentStepDesc}>{currentStep.description}</p>
          </div>

          <div className={styles.stepHighlights}>
            {currentStep.highlights.map((highlight, index) => (
              <span key={index} className={styles.highlight}>
                ✨ {highlight}
              </span>
            ))}
          </div>

          {/* Simple Code Example with InlineSnippet */}
          <div className={styles.codeExample}>
            <InlineSnippet 
              snipt={currentStep.code}
              language={currentStep.language}
              className={styles.quickStartCode}
            />
          </div>

          {/* Navigation */}
          <div className={styles.stepNavigation}>
            <button 
              className={`${styles.navButton} ${styles.prevButton}`}
              onClick={() => setActiveStep(Math.max(0, activeStep - 1))}
              disabled={activeStep === 0}
            >
              ← Previous
            </button>
            
            <div className={styles.stepDots}>
              {steps.map((_, index) => (
                <button
                  key={index}
                  className={`${styles.dot} ${activeStep === index ? styles.activeDot : ''}`}
                  onClick={() => setActiveStep(index)}
                />
              ))}
            </div>

            <button 
              className={`${styles.navButton} ${styles.nextButton}`}
              onClick={() => setActiveStep(Math.min(steps.length - 1, activeStep + 1))}
              disabled={activeStep === steps.length - 1}
            >
              {activeStep === steps.length - 1 ? '🎉 Done!' : 'Next →'}
            </button>
          </div>
        </div>

        {/* Call to Action */}
        {activeStep === steps.length - 1 && (
          <div className={styles.completionMessage}>
            <h4 className={styles.completionTitle}>🎉 You're all set!</h4>
            <p className={styles.completionText}>
              You've successfully set up EasyValidate. Ready to explore more features?
            </p>
            <div className={styles.completionActions}>
              <button className={styles.primaryButton}>
                View All Attributes
              </button>
              <button className={styles.secondaryButton}>
                Explore Examples
              </button>
            </div>
          </div>
        )}
      </div>
    </section>
  );
}

export default QuickStartSection;
