import React from 'react';
import styles from './HowItWorksSection.module.css';

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

export default HowItWorksSection;
