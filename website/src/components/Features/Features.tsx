import styles from './Features.module.css';

type Feature = {
  icon: string;
  title: string;
  description: string;
  highlight?: boolean;
};

function Features() {
  const features: Feature[] = [
    {
      icon: '⚡',
      title: 'High Performance',
      description: 'Source generators ensure zero runtime reflection and maximum performance.',
      highlight: true
    },
    {
      icon: '🎯',
      title: 'Type Safe',
      description: 'Full TypeScript-like intellisense and compile-time validation checking.',
      highlight: true
    },
    {
      icon: '🔧',
      title: 'Extensible',
      description: 'Create custom validation attributes and formatters with ease.',
      highlight: true
    },
    {
      icon: '📝',
      title: 'Rich Attributes',
      description: 'Comprehensive set of built-in validation attributes for common scenarios.'
    },
    {
      icon: '🔍',
      title: 'Clear Errors',
      description: 'Detailed error messages with property paths and custom formatting.'
    },
    {
      icon: '⚙️',
      title: 'Zero Config',
      description: 'Works out of the box with sensible defaults and minimal setup.'
    },
    {
      icon: '🚀',
      title: 'Modern API',
      description: 'Clean, intuitive API designed for modern .NET development practices.'
    },
    {
      icon: '🔄',
      title: 'Composable',
      description: 'Chain validations and combine multiple rules for complex scenarios.'
    }
  ];

  return (
    <section id="features" className={styles.features}>
      <div className={styles.container}>
        <div className={styles.header}>
          <h2 className={styles.title}>Why Choose EasyValidate?</h2>
          <p className={styles.subtitle}>
            Built for modern .NET applications with performance and developer experience in mind.
          </p>
        </div>
        
        <div className={styles.grid}>
          {features.map((feature, index) => (
            <div 
              key={index} 
              className={`${styles.feature} ${feature.highlight ? styles.highlighted : ''}`}
            >
              <div className={styles.icon}>{feature.icon}</div>
              <h3 className={styles.featureTitle}>{feature.title}</h3>
              <p className={styles.featureDescription}>{feature.description}</p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
}

export default Features;
