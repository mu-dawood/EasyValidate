import styles from './Footer.module.css';

function Footer() {
  return (
    <footer className={styles.footer}>
      <div className={styles.container}>
        <div className={styles.content}>
          <div className={styles.brand}>
            <h3 className={styles.brandTitle}>EasyValidate</h3>
            <p className={styles.brandDescription}>
              Modern validation for .NET applications
            </p>
          </div>
          
          <div className={styles.links}>
            <div className={styles.linkSection}>
              <h4 className={styles.linkTitle}>Documentation</h4>
              <ul className={styles.linkList}>
                <li><a href="#getting-started" className={styles.link}>Getting Started</a></li>
                <li><a href="#examples" className={styles.link}>Examples</a></li>
                <li><a href="#docs" className={styles.link}>API Reference</a></li>
                <li><a href="#migration" className={styles.link}>Migration Guide</a></li>
              </ul>
            </div>
            
            <div className={styles.linkSection}>
              <h4 className={styles.linkTitle}>Community</h4>
              <ul className={styles.linkList}>
                <li><a href="https://github.com/mu-dawood/EasyValidate" className={styles.link}>GitHub</a></li>
                <li><a href="https://github.com/mu-dawood/EasyValidate/issues" className={styles.link}>Issues</a></li>
                <li><a href="https://github.com/mu-dawood/EasyValidate/discussions" className={styles.link}>Discussions</a></li>
                <li><a href="https://nuget.org/packages/EasyValidate" className={styles.link}>NuGet</a></li>
              </ul>
            </div>
            
            <div className={styles.linkSection}>
              <h4 className={styles.linkTitle}>Resources</h4>
              <ul className={styles.linkList}>
                <li><a href="#changelog" className={styles.link}>Changelog</a></li>
                <li><a href="#roadmap" className={styles.link}>Roadmap</a></li>
                <li><a href="#contributing" className={styles.link}>Contributing</a></li>
                <li><a href="#license" className={styles.link}>License</a></li>
              </ul>
            </div>
          </div>
        </div>
        
        <div className={styles.bottom}>
          <div className={styles.copyright}>
            <p>&copy; 2025 EasyValidate. Released under the MIT License.</p>
          </div>
          <div className={styles.badges}>
            <img src="https://img.shields.io/nuget/v/EasyValidate" alt="NuGet Version" />
            <img src="https://img.shields.io/github/stars/mu-dawood/EasyValidate" alt="GitHub Stars" />
          </div>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
