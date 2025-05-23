import React from 'react';
import styles from './AttributeCard.module.css';

interface AttributeCardProps {
  name: string;
  category: string;
  description: string;
  example: string;
  parameters?: string[];
}

export default function AttributeCard({ 
  name, 
  category, 
  description, 
  example, 
  parameters = [] 
}: AttributeCardProps) {
  return (
    <div className={styles.card}>
      <div className={styles.header}>
        <div className={styles.nameContainer}>
          <h3 className={styles.name}>{name}</h3>
          <span className={styles.category}>{category}</span>
        </div>
      </div>
      
      <p className={styles.description}>{description}</p>
      
      {parameters.length > 0 && (
        <div className={styles.parameters}>
          <h4 className={styles.parametersTitle}>Parameters:</h4>
          <ul className={styles.parametersList}>
            {parameters.map((param, index) => (
              <li key={index} className={styles.parameter}>{param}</li>
            ))}
          </ul>
        </div>
      )}
      
      <div className={styles.example}>
        <h4 className={styles.exampleTitle}>Example:</h4>
        <pre className={styles.code}>
          <code>{example}</code>
        </pre>
      </div>
    </div>
  );
}
