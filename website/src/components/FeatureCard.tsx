import React from 'react';
import styles from './FeatureCard.module.css';

interface FeatureCardProps {
  icon: string;
  title: string;
  description: string;
  color?: 'primary' | 'secondary' | 'accent' | 'success' | 'warning';
}

export default function FeatureCard({ 
  icon, 
  title, 
  description, 
  color = 'primary' 
}: FeatureCardProps) {
  return (
    <div className={`${styles.card} ${styles[color]}`}>
      <div className={styles.icon}>{icon}</div>
      <h3 className={styles.title}>{title}</h3>
      <p className={styles.description}>{description}</p>
    </div>
  );
}
