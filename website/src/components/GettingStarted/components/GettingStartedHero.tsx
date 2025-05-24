import styles from '../GettingStarted.module.css';

function GettingStartedHero() {
  return (
    <div className={styles.hero}>
      <div className={styles.heroContent}>
        <span className={styles.badge}>🚀 Quick Start</span>
        <h2 className={styles.title}>Start Validating in 3 Steps</h2>
        <p className={styles.subtitle}>
          From zero to hero in minutes. EasyValidate makes model validation 
          effortless with compile-time code generation.
        </p>
      </div>
    </div>
  );
}

export default GettingStartedHero;
