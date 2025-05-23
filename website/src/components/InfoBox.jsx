import React from 'react';
import styles from './InfoBox.module.css';

const icons = {
  tip: '💡',
  note: 'ℹ️',
  warning: '⚠️',
  danger: '🚨',
  success: '✅'
};

export default function InfoBox({ type, title, children }) {
  return (
    <div className={`${styles.infoBox} ${styles[type]}`}>
      <div className={styles.header}>
        <span className={styles.icon}>{icons[type]}</span>
        {title && <span className={styles.title}>{title}</span>}
      </div>
      <div className={styles.content}>
        {children}
      </div>
    </div>
  );
}
