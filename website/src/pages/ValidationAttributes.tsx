import { useState } from 'react';
import {
  StringAttributes,
  NumericAttributes,
  DateAttributes,
  CollectionAttributes,
  GeneralAttributes
} from '../components/ValidationAttributes';
import styles from './ValidationAttributes.module.css';

function ValidationAttributes() {
  const [activeSection, setActiveSection] = useState<string>('all');

  const sections = [
    { id: 'all', label: 'All Attributes', icon: '📋' },
    { id: 'string', label: 'String', icon: '📝' },
    { id: 'numeric', label: 'Numeric', icon: '🔢' },
    { id: 'date', label: 'Date & Time', icon: '📅' },
    { id: 'collection', label: 'Collections', icon: '📚' },
    { id: 'general', label: 'General', icon: '⚙️' },
  ];

  const renderContent = () => {
    switch (activeSection) {
      case 'string':
        return <StringAttributes />;
      case 'numeric':
        return <NumericAttributes />;
      case 'date':
        return <DateAttributes />;
      case 'collection':
        return <CollectionAttributes />;
      case 'general':
        return <GeneralAttributes />;
      default:
        return (
          <>
            <StringAttributes />
            <NumericAttributes />
            <DateAttributes />
            <CollectionAttributes />
            <GeneralAttributes />
          </>
        );
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Validation Attributes</h1>
        <p className={styles.subtitle}>
          Comprehensive guide to all 67+ validation attributes available in EasyValidate
        </p>
      </div>

      <div className={styles.navigation}>
        <div className={styles.navTabs}>
          {sections.map((section) => (
            <button
              key={section.id}
              className={`${styles.navTab} ${activeSection === section.id ? styles.active : ''}`}
              onClick={() => setActiveSection(section.id)}
            >
              <span className={styles.navIcon}>{section.icon}</span>
              <span className={styles.navLabel}>{section.label}</span>
            </button>
          ))}
        </div>
      </div>

      <div className={styles.content}>
        {renderContent()}
      </div>

      <div className={styles.footer}>
        <div className={styles.stats}>
          <div className={styles.statItem}>
            <span className={styles.statNumber}>67+</span>
            <span className={styles.statLabel}>Total Attributes</span>
          </div>
          <div className={styles.statItem}>
            <span className={styles.statNumber}>5</span>
            <span className={styles.statLabel}>Categories</span>
          </div>
          <div className={styles.statItem}>
            <span className={styles.statNumber}>100%</span>
            <span className={styles.statLabel}>Type Safe</span>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ValidationAttributes;
