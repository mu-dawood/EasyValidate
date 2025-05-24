import React from 'react';
import clsx from 'clsx';
import Link from '@docusaurus/Link';
import styles from './CallToActionSection.module.css';

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
              to="https://github.com/mu-dawood/EasyValidate">
              GitHub Repository
            </Link>
            <Link
              className={clsx('button', 'button--secondary', 'button--outline', 'button--lg', styles.ctaButton)}
              to="/docs/examples">
              Examples
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}

export default CallToActionSection;
