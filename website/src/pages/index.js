import React from 'react';
import clsx from 'clsx';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import styles from './index.module.css';

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
              Modern, fast, and extensible attribute-based validation for .NET
            </p>
            <p className={styles.heroDescription}>
              Build robust applications with compile-time validation, zero reflection overhead, 
              and intelligent IDE support powered by source generators.
            </p>
            <div className={styles.heroButtons}>
              <Link
                className={clsx('button', 'button--primary', 'button--lg', styles.heroButton)}
                to="/docs/getting-started">
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
{`public class UserModel
{
    [Required, StringLength(50)]
    public string Name { get; set; }
    
    [Email]
    public string Email { get; set; }
    
    [Range(18, 120)]
    public int Age { get; set; }
}`}
              </pre>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
}

function FeatureCard({icon, title, description, gradient}) {
  return (
    <div className={clsx(styles.featureCard, styles[gradient])}>
      <div className={styles.featureIcon}>
        {icon}
      </div>
      <h3 className={styles.featureTitle}>{title}</h3>
      <p className={styles.featureDescription}>{description}</p>
    </div>
  );
}

function FeaturesSection() {
  return (
    <section className={styles.featuresSection}>
      <div className={styles.container}>
        <div className={styles.sectionHeader}>
          <h2 className={styles.sectionTitle}>Why Choose EasyValidate?</h2>
          <p className={styles.sectionSubtitle}>
            Built for modern .NET development with performance and developer experience in mind
          </p>
        </div>
        <div className={styles.featuresGrid}>
          <FeatureCard
            icon="🚀"
            title="High Performance"
            description="Zero-reflection validation using source generators. Compile-time code generation means runtime speed."
            gradient="gradientBlue"
          />
          <FeatureCard
            icon="🔧"
            title="Developer Friendly"
            description="Rich IDE support with IntelliSense, error highlighting, and automatic fixes. Validation errors at compile-time."
            gradient="gradientPurple"
          />
          <FeatureCard
            icon="🎯"
            title="Attribute-Based"
            description="Clean, declarative validation using familiar attributes. Extensive built-in validators for common scenarios."
            gradient="gradientGreen"
          />
          <FeatureCard
            icon="🔄"
            title="Extensible"
            description="Create custom validators with ease. Compose complex validation rules and reuse them across your application."
            gradient="gradientOrange"
          />
          <FeatureCard
            icon="🌍"
            title="Localization Ready"
            description="Full internationalization support with resource-based error messages and culture-aware formatting."
            gradient="gradientRed"
          />
          <FeatureCard
            icon="📦"
            title="NuGet Ready"
            description="Easy installation via NuGet. Compatible with .NET 6+, ASP.NET Core, and popular frameworks."
            gradient="gradientTeal"
          />
        </div>
      </div>
    </section>
  );
}

function StatsSection() {
  return (
    <section className={styles.statsSection}>
      <div className={styles.container}>
        <div className={styles.statsGrid}>
          <div className={styles.statItem}>
            <div className={styles.statNumber}>10x</div>
            <div className={styles.statLabel}>Faster than reflection</div>
          </div>
          <div className={styles.statItem}>
            <div className={styles.statNumber}>Zero</div>
            <div className={styles.statLabel}>Runtime overhead</div>
          </div>
          <div className={styles.statItem}>
            <div className={styles.statNumber}>100%</div>
            <div className={styles.statLabel}>Compile-time safety</div>
          </div>
          <div className={styles.statItem}>
            <div className={styles.statNumber}>50+</div>
            <div className={styles.statLabel}>Built-in validators</div>
          </div>
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
          <h2 className={styles.ctaTitle}>Ready to validate like a pro?</h2>
          <p className={styles.ctaDescription}>
            Join thousands of developers building better .NET applications with EasyValidate
          </p>
          <div className={styles.ctaButtons}>
            <Link
              className={clsx('button', 'button--primary', 'button--lg', styles.ctaButton)}
              to="/docs/getting-started">
              Start Building
            </Link>
            <Link
              className={clsx('button', 'button--secondary', 'button--outline', 'button--lg', styles.ctaButton)}
              to="/docs/examples">
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
      title={`${siteConfig.title} - Modern .NET Validation`}
      description="Modern, fast, and extensible attribute-based validation for .NET with source generators">
      <HeroSection />
      <main>
        <FeaturesSection />
        <StatsSection />
        <CallToActionSection />
      </main>
    </Layout>
  );
}
