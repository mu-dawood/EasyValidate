import styles from './DocsOverview.module.css';

function DocsOverview() {
  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>EasyValidate Documentation</h1>
        <p className={styles.subtitle}>
          A comprehensive guide to building robust validation with EasyValidate
        </p>
      </div>

      <div className={styles.grid}>
        <div className={styles.card}>
          <div className={styles.cardIcon}>🚀</div>
          <h3 className={styles.cardTitle}>Getting Started</h3>
          <p className={styles.cardDescription}>
            Quick setup guide to get EasyValidate running in your project in minutes.
          </p>
          <a href="/docs/getting-started" className={styles.cardLink}>
            Start Here →
          </a>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>💡</div>
          <h3 className={styles.cardTitle}>Real-World Examples</h3>
          <p className={styles.cardDescription}>
            Practical examples showing how to validate common scenarios like users, products, and orders.
          </p>
          <a href="/docs/examples" className={styles.cardLink}>
            View Examples →
          </a>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>🏷️</div>
          <h3 className={styles.cardTitle}>Validation Attributes</h3>
          <p className={styles.cardDescription}>
            Complete reference of all available validation attributes and their usage.
          </p>
          <a href="/docs/attributes" className={styles.cardLink}>
            Browse Attributes →
          </a>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>⚙️</div>
          <h3 className={styles.cardTitle}>Advanced Usage</h3>
          <p className={styles.cardDescription}>
            Custom validators, complex scenarios, and advanced configuration options.
          </p>
          <a href="/docs/advanced" className={styles.cardLink}>
            Learn Advanced →
          </a>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>📚</div>
          <h3 className={styles.cardTitle}>API Reference</h3>
          <p className={styles.cardDescription}>
            Detailed API documentation for all classes, methods, and interfaces.
          </p>
          <a href="/docs/api" className={styles.cardLink}>
            API Docs →
          </a>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>❓</div>
          <h3 className={styles.cardTitle}>FAQ & Troubleshooting</h3>
          <p className={styles.cardDescription}>
            Common questions and solutions to help you solve issues quickly.
          </p>
          <a href="/docs/faq" className={styles.cardLink}>
            Get Help →
          </a>
        </div>
      </div>

      <div className={styles.features}>
        <h2 className={styles.featuresTitle}>Why Choose EasyValidate?</h2>
        <div className={styles.featuresList}>
          <div className={styles.feature}>
            <div className={styles.featureIcon}>⚡</div>
            <div>
              <h4>High Performance</h4>
              <p>Optimized for speed with minimal overhead and efficient validation logic.</p>
            </div>
          </div>
          <div className={styles.feature}>
            <div className={styles.featureIcon}>🎯</div>
            <div>
              <h4>Type Safe</h4>
              <p>Full TypeScript support with compile-time validation and IntelliSense.</p>
            </div>
          </div>
          <div className={styles.feature}>
            <div className={styles.featureIcon}>🔧</div>
            <div>
              <h4>Extensible</h4>
              <p>Easy to extend with custom validators and flexible configuration options.</p>
            </div>
          </div>
          <div className={styles.feature}>
            <div className={styles.featureIcon}>📦</div>
            <div>
              <h4>Lightweight</h4>
              <p>Minimal dependencies and small bundle size for better performance.</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default DocsOverview;
