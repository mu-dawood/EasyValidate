import { Outlet, Link, useLocation } from 'react-router-dom';
import styles from './DocumentationLayout.module.css';

function DocumentationLayout() {
  const location = useLocation();

  const navigationItems = [
    { path: '/docs', label: 'Overview', icon: '📋' },
    { path: '/docs/getting-started', label: 'Getting Started', icon: '🚀' },
    { path: '/docs/examples', label: 'Real-World Examples', icon: '💡' },
    { path: '/docs/attributes', label: 'Validation Attributes', icon: '🏷️' },
    { path: '/docs/advanced', label: 'Advanced Usage', icon: '⚙️' },
    { path: '/docs/api', label: 'API Reference', icon: '📚' },
  ];

  return (
    <div className={styles.layout}>
      <nav className={styles.sidebar}>
        <div className={styles.sidebarHeader}>
          <h2 className={styles.sidebarTitle}>Documentation</h2>
          <p className={styles.sidebarSubtitle}>EasyValidate Guide</p>
        </div>
        
        <ul className={styles.navigationList}>
          {navigationItems.map((item) => (
            <li key={item.path} className={styles.navigationItem}>
              <Link
                to={item.path}
                className={`${styles.navigationLink} ${
                  location.pathname === item.path ? styles.active : ''
                }`}
              >
                <span className={styles.navigationIcon}>{item.icon}</span>
                <span className={styles.navigationLabel}>{item.label}</span>
              </Link>
            </li>
          ))}
        </ul>

        <div className={styles.sidebarFooter}>
          <Link to="/" className={styles.backToHome}>
            ← Back to Home
          </Link>
        </div>
      </nav>

      <main className={styles.content}>
        <Outlet />
      </main>
    </div>
  );
}

export default DocumentationLayout;
