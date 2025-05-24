import { Link, useLocation } from 'react-router-dom';
import styles from './Header.module.css';

function Header() {
  const location = useLocation();
  const isDocsPage = location.pathname.startsWith('/docs');

  return (
    <header className={styles.header}>
      <div className={styles.container}>
        <div className={styles.logo}>
          <Link to="/" className={styles.logoLink}>
            <h1>EasyValidate</h1>
          </Link>
        </div>
        <nav className={styles.nav}>
          {!isDocsPage && (
            <>
              <a href="#features" className={styles.navLink}>Features</a>
              <a href="#getting-started" className={styles.navLink}>Getting Started</a>
            </>
          )}
          <Link to="/docs" className={styles.navLink}>Documentation</Link>
          <Link to="/docs/examples" className={styles.navLink}>Examples</Link>
          <a 
            href="https://github.com/mu-dawood/EasyValidate" 
            className={styles.githubLink}
            target="_blank"
            rel="noopener noreferrer"
          >
            GitHub
          </a>
        </nav>
      </div>
    </header>
  );
}

export default Header;
