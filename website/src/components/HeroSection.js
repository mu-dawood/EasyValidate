import React from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import CodeWindow from './CodeWindow';
import styles from './HeroSection.module.css';

function HeroSection() {
  const { siteConfig } = useDocusaurusContext();
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
                <span className={styles.githubIcon}>
                  ⭐
                </span>
                GitHub
              </Link>
            </div>
          </div>
          <div className={styles.heroVisual}>
            <CodeWindow 
              fileName="UserModel.cs"
              language="csharp"
              variant="light"
              showCopyButton={true}
              className={styles.heroCodeWindow}
            >
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
            </CodeWindow>
          </div>
        </div>
      </div>
    </header>
  );
}

export default HeroSection;
