import React from 'react';
import styles from './AttributesSection.module.css';

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

export default AttributesSection;
