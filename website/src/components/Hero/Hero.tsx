import CodeWindow, { type Window } from '../CodeWindow';
import styles from './Hero.module.css';

function Hero() {
  const heroCode: Window[] = [
    {
      fileName: 'User.cs',
      language: 'csharp',
      snipt: `public class User
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Email]
    public string Email { get; set; }

    [Range(18, 120)]
    public int Age { get; set; }
}`,
      active: true
    }
  ];
  return (
    <section className={styles.hero}>
      <div className={styles.container}>
        <div className={styles.content}>
          <h1 className={styles.title}>
            Modern Validation for .NET
          </h1>
          <p className={styles.subtitle}>
            Fast, flexible, and intuitive attribute-based validation library that makes validating your models effortless.
          </p>
          <div className={styles.badges}>
            <span className={styles.badge}>⚡ High Performance</span>
            <span className={styles.badge}>🎯 Type Safe</span>
            <span className={styles.badge}>🔧 Extensible</span>
          </div>
          <div className={styles.actions}>
            <a href="/docs" className={styles.primaryButton}>
              Get Started
            </a>
            <a href="/docs/examples" className={styles.secondaryButton}>
              Examples
            </a>
          </div>
        </div>
        <div className={styles.codePreview}>
          <CodeWindow windows={heroCode} variant="hero" />
        </div>
      </div>
    </section>
  );
}

export default Hero;
