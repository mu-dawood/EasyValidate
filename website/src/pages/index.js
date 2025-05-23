import React, { useEffect } from 'react';
import clsx from 'clsx';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import styles from './index.module.css';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';

// Enhanced code block component with Prism.js syntax highlighting
function CodeBlock({ children, language = 'csharp', className = '' }) {
  useEffect(() => {
    Prism.highlightAll();
  }, []);

  return (
    <div className={clsx(styles.codeBlock, className)}>
      <pre>
        <code className={`language-${language}`}>
          {children}
        </code>
      </pre>
    </div>
  );
}

function HeroSection() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className={styles.heroBanner}>
      <div className={styles.heroContainer}>
        <div className={styles.heroContent}>
          <div className={styles.heroText}>
            <h1 className={styles.heroTitle}>
              <span className={styles.heroTitleGradient}>EasyValidate</span>
            </h1>
            <p className={styles.heroSubtitle}>
              Source Generator for Attribute-Based Validation in .NET
            </p>
            <p className={styles.heroDescription}>
              Automatically generates validation methods from attributes at compile-time. 
              No reflection, no performance overhead, just fast and reliable validation.
            </p>
            <div className={styles.heroButtons}>
              <Link
                className={clsx('button', 'button--primary', 'button--lg', styles.heroButton)}
                to="/docs/intro">
                Get Started
              </Link>
              <Link
                className={clsx('button', 'button--secondary', 'button--outline', 'button--lg', styles.heroButton)}
                to="https://github.com/EasyValidate/EasyValidate">
                <span className={styles.githubIcon}>⭐</span>
                GitHub
              </Link>
            </div>
          </div>
          <div className={styles.heroVisual}>
            <div className={styles.codePreview}>
              <div className={styles.codeHeader}>
                <div className={styles.codeDots}>
                  <span></span>
                  <span></span>
                  <span></span>
                </div>
                <span className={styles.codeTitle}>UserModel.cs</span>
              </div>
              <pre className={styles.codeContent}>
                <CodeBlock>
{`// Your model with attributes
public partial class UserModel
{
    [Required, StringLength(50)]
    public string Name { get; set; }
    
    [Email]
    public string Email { get; set; }
    
    [Range(18, 120)]
    public int Age { get; set; }
}

// Generated at compile-time
public partial class UserModel : IValidate
{
    public ValidationResult Validate()
    {
        var result = new ValidationResult();
        // Validation logic here...
        return result;
    }
}`}
                </CodeBlock>
              </pre>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}

function HowItWorksSection() {
  const features = [
    {
      icon: "🏷️",
      title: "Add Attributes",
      description: "Decorate your models with validation attributes like [Required], [Email], [Range], etc."
    },
    {
      icon: "⚡",
      title: "Source Generator",
      description: "EasyValidate generates validation code at compile-time, no runtime reflection needed."
    },
    {
      icon: "🎯",
      title: "IValidate Interface",
      description: "Your models implement IValidate interface following SOLID principles for clean architecture."
    },
    {
      icon: "🔄",
      title: "Nested Validation",
      description: "Automatically validates nested objects and collections with full support for complex types."
    },
    {
      icon: "📊",
      title: "ValidationResult",
      description: "Returns detailed ValidationResult objects with property-specific error messages."
    },
    {
      icon: "🔍",
      title: "Diagnostic Analyzers",
      description: "Built-in analyzers ensure attribute compatibility and catch issues at design-time."
    }
  ];

  return (
    <section className={styles.featuresSection}>
      <div className={styles.container}>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>How It Works</h2>
          <p className={styles.sectionSubtitle}>
            EasyValidate uses source generators to create fast, compile-time validation
          </p>
        </div>
        <div className={styles.featuresGrid}>
          {features.map((feature, idx) => (
            <div key={idx} className={styles.featureCard}>
              <div className={styles.featureIcon}>
                {feature.icon}
              </div>
              <h3 className={styles.featureTitle}>{feature.title}</h3>
              <p className={styles.featureDescription}>{feature.description}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function AttributesSection() {
  const attributeCategories = [
    {
      icon: "📝",
      title: "String Validation",
      description: "Validate strings with length, format, and content rules",
      example: "[Required, StringLength(100), Email]"
    },
    {
      icon: "🔢",
      title: "Numeric Validation",
      description: "Range checks, comparisons, and numeric constraints",
      example: "[Range(1, 100), GreaterThan(0)]"
    },
    {
      icon: "📅",
      title: "Date Validation",
      description: "Date ranges, comparisons, and temporal constraints",
      example: "[DateRange(\"2020-01-01\", \"2030-12-31\")]"
    },
    {
      icon: "📋",
      title: "Collection Validation",
      description: "Validate arrays, lists, and collection properties",
      example: "[CollectionNotEmpty, MaxLength(10)]"
    },
    {
      icon: "✅",
      title: "General Validation",
      description: "Required fields, null checks, and basic constraints",
      example: "[Required, NotNull, NotDefault]"
    },
    {
      icon: "🎨",
      title: "Custom Attributes",
      description: "Create your own validation attributes easily",
      example: "[CustomValidator(typeof(MyValidator))]"
    }
  ];

  return (
    <section className={styles.attributesSection}>
      <div className={styles.container}>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>Built-in Validation Attributes</h2>
          <p className={styles.sectionSubtitle}>
            Comprehensive set of validation attributes covering most common scenarios
          </p>
        </div>
        <div className={styles.attributesGrid}>
          {attributeCategories.map((category, idx) => (
            <div key={idx} className={styles.attributeCard}>
              <div className={styles.attributeIcon}>
                {category.icon}
              </div>
              <h3>{category.title}</h3>
              <p>{category.description}</p>
              <div className={styles.attributeExample}>
                <code>{category.example}</code>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function QuickStartSection() {
  const steps = [
    {
      title: "Install Package",
      description: "Add EasyValidate to your project via NuGet Package Manager",
      code: "dotnet add package EasyValidate",
      language: "bash"
    },
    {
      title: "Add Attributes",
      description: "Decorate your model properties with validation attributes",
      code: `public partial class User
{
    [Required, StringLength(50)]
    public string Name { get; set; }
    
    [Email]
    public string Email { get; set; }
}`,
      language: "csharp"
    },
    {
      title: "Validate",
      description: "Use the generated validation methods in your application",
      code: `var user = new User { Name = "John", Email = "invalid-email" };
var result = user.Validate();

if (!result.IsValid)
{
    foreach (var error in result.Errors)
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
}`,
      language: "csharp"
    }
  ];

  return (
    <section className={styles.quickStartSection}>
      <div className={styles.container}>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>Quick Start</h2>
          <p className={styles.sectionSubtitle}>
            Get up and running with EasyValidate in just 3 simple steps
          </p>
        </div>
        <div className={styles.quickStartGrid}>
          {steps.map((step, idx) => (
            <div key={idx} className={styles.quickStartCard}>
              <div className={styles.stepNumber}>{idx + 1}</div>
              <h3>{step.title}</h3>
              <p>{step.description}</p>
              <div className={styles.codeBlock}>
                <CodeBlock language={step.language}>{step.code}</CodeBlock>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

function CallToActionSection() {
  return (
    <section className={styles.ctaSection}>
      <div className={styles.container}>
        <div className={styles.ctaContent}>
          <h2 className={styles.ctaTitle}>Ready to validate with confidence?</h2>
          <p className={styles.ctaDescription}>
            Start building better .NET applications with compile-time validation
          </p>
          <div className={styles.ctaButtons}>
            <Link
              className={clsx('button', 'button--primary', 'button--lg', styles.ctaButton)}
              to="/docs/intro">
              Read Documentation
            </Link>
            <Link
              className={clsx('button', 'button--secondary', 'button--outline', 'button--lg', styles.ctaButton)}
              to="/docs/quickstart">
              View Examples
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}

export default function Home() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout
      title={`${siteConfig.title} - Source Generator for .NET Validation`}
      description="Source Generator for Attribute-Based Validation in .NET. Automatically generates validation methods from attributes at compile-time.">
      <HeroSection />
      <main>
        <HowItWorksSection />
        <AttributesSection />
        <QuickStartSection />
        <CallToActionSection />
      </main>
    </Layout>
  );
}