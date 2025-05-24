import styles from '../GettingStarted.module.css';

function CallToAction() {
  return (
    <div className={styles.cta}>
      <div className={styles.ctaContent}>
        <h3 className={styles.ctaTitle}>Ready to Get Started?</h3>
        <p className={styles.ctaDescription}>
          Join thousands of developers building better .NET applications with EasyValidate
        </p>
        <div className={styles.ctaButtons}>
          <a href="/docs" className={styles.primaryButton}>
            <span>📚</span>
            Read Full Documentation
          </a>
          <a href="/docs/examples" className={styles.secondaryButton}>
            <span>🧪</span>
            Browse Examples
          </a>
          <a href="https://github.com/mu-dawood/EasyValidate" className={styles.githubButton}>
            <span>⭐</span>
            Star on GitHub
          </a>
        </div>
      </div>
    </div>
  );
}

export default CallToAction;
