import React from 'react';
import styles from './DocSection.module.css';

export default function DocSection({ 
  title, 
  subtitle, 
  icon, 
  children, 
  background = 'white' 
}) {
  return (
    <section className={`${styles.section} ${styles[background]}`}>
      <div className={styles.header}>
        {icon && <span className={styles.icon}>{icon}</span>}
        <div className={styles.titleContainer}>
          <h2 className={styles.title}>{title}</h2>
          {subtitle && <p className={styles.subtitle}>{subtitle}</p>}
        </div>
      </div>
      <div className={styles.content}>
        {children}
      </div>
    </section>
  );
}
