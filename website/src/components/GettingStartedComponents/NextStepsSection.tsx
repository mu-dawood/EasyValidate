import { Link } from 'react-router-dom';
import styles from './GettingStartedComponents.module.css';

interface NextStep {
  id: string;
  icon: string;
  title: string;
  description: string;
  path: string;
  difficulty: 'Beginner' | 'Intermediate' | 'Advanced';
  estimatedTime: string;
  highlights: string[];
}

function NextStepsSection() {
  const nextSteps: NextStep[] = [
    {
      id: 'examples',
      icon: '💡',
      title: 'Real-World Examples',
      description: 'Explore practical validation scenarios and implementation patterns across different domains.',
      path: '/docs/examples',
      difficulty: 'Beginner',
      estimatedTime: '15 min',
      highlights: [
        'User registration forms',
        'E-commerce product validation',
        'API request validation',
        'Complex nested objects'
      ]
    },
    {
      id: 'attributes',
      icon: '🏷️',
      title: 'Validation Attributes Reference',
      description: 'Complete guide to all 67+ built-in validation attributes organized by category.',
      path: '/docs/attributes',
      difficulty: 'Beginner',
      estimatedTime: '20 min',
      highlights: [
        'String validation attributes',
        'Numeric and date validation',
        'Collection validation',
        'Custom attribute creation'
      ]
    },
    {
      id: 'advanced',
      icon: '⚙️',
      title: 'Advanced Usage',
      description: 'Master advanced features like custom validators, conditional validation, and performance optimization.',
      path: '/docs/advanced',
      difficulty: 'Advanced',
      estimatedTime: '30 min',
      highlights: [
        'Custom validation logic',
        'Conditional validation rules',
        'Performance optimization',
        'Testing strategies'
      ]
    },
    {
      id: 'api',
      icon: '📚',
      title: 'API Reference',
      description: 'Detailed documentation for all classes, methods, and interfaces in the EasyValidate library.',
      path: '/docs/api',
      difficulty: 'Intermediate',
      estimatedTime: '25 min',
      highlights: [
        'Core validation classes',
        'Attribute base classes',
        'Result and error types',
        'Extension methods'
      ]
    }
  ];

  const getDifficultyColor = (difficulty: string) => {
    switch (difficulty) {
      case 'Beginner': return '#22c55e';
      case 'Intermediate': return '#f59e0b';
      case 'Advanced': return '#ef4444';
      default: return '#6b7280';
    }
  };

  return (
    <section className={styles.section}>
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>
          <span className={styles.sectionIcon}>📚</span>
          What's Next?
        </h2>
        <p className={styles.sectionDescription}>
          Continue your EasyValidate journey with these comprehensive guides and references
        </p>
      </div>

      <div className={styles.nextStepsContainer}>
        <div className={styles.nextStepsGrid}>
          {nextSteps.map((step) => (
            <Link 
              key={step.id}
              to={step.path}
              className={styles.nextStepCard}
            >
              <div className={styles.nextStepHeader}>
                <div className={styles.nextStepIcon}>{step.icon}</div>
                <div className={styles.nextStepMeta}>
                  <div className={styles.nextStepMetaRow}>
                    <span 
                      className={styles.difficultyBadge}
                      style={{ backgroundColor: getDifficultyColor(step.difficulty) }}
                    >
                      {step.difficulty}
                    </span>
                    <span className={styles.estimatedTime}>
                      <span className={styles.timeIcon}>🕒</span>
                      {step.estimatedTime}
                    </span>
                  </div>
                </div>
              </div>

              <div className={styles.nextStepContent}>
                <h3 className={styles.nextStepTitle}>{step.title}</h3>
                <p className={styles.nextStepDescription}>{step.description}</p>

                <div className={styles.nextStepHighlights}>
                  <div className={styles.highlightsTitle}>What you'll learn:</div>
                  <ul className={styles.highlightsList}>
                    {step.highlights.map((highlight, index) => (
                      <li key={index} className={styles.highlightItem}>
                        <span className={styles.highlightIcon}>▸</span>
                        {highlight}
                      </li>
                    ))}
                  </ul>
                </div>
              </div>

              <div className={styles.nextStepAction}>
                <span className={styles.actionText}>Start Learning</span>
                <span className={styles.actionIcon}>→</span>
              </div>
            </Link>
          ))}
        </div>

        {/* Learning Path */}
        <div className={styles.learningPath}>
          <h3 className={styles.learningPathTitle}>
            <span className={styles.pathIcon}>🗺️</span>
            Recommended Learning Path
          </h3>
          <div className={styles.pathSteps}>
            <div className={styles.pathStep}>
              <div className={styles.pathStepNumber}>1</div>
              <div className={styles.pathStepContent}>
                <strong>Start with Examples</strong> - See real-world usage patterns
              </div>
            </div>
            <div className={styles.pathConnector}></div>
            <div className={styles.pathStep}>
              <div className={styles.pathStepNumber}>2</div>
              <div className={styles.pathStepContent}>
                <strong>Explore Attributes</strong> - Learn all available validation options
              </div>
            </div>
            <div className={styles.pathConnector}></div>
            <div className={styles.pathStep}>
              <div className={styles.pathStepNumber}>3</div>
              <div className={styles.pathStepContent}>
                <strong>Master Advanced Features</strong> - Custom validators and optimization
              </div>
            </div>
            <div className={styles.pathConnector}></div>
            <div className={styles.pathStep}>
              <div className={styles.pathStepNumber}>4</div>
              <div className={styles.pathStepContent}>
                <strong>Reference API Docs</strong> - Deep dive into implementation details
              </div>
            </div>
          </div>
        </div>

        {/* Quick Links */}
        <div className={styles.quickLinks}>
          <h3 className={styles.quickLinksTitle}>Quick Access</h3>
          <div className={styles.quickLinksGrid}>
            <a 
              href="https://github.com/yourusername/easyvalidate" 
              className={styles.quickLink}
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className={styles.quickLinkIcon}>📦</span>
              <span className={styles.quickLinkText}>Source Code</span>
            </a>
            <a 
              href="https://www.nuget.org/packages/EasyValidate" 
              className={styles.quickLink}
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className={styles.quickLinkIcon}>🔗</span>
              <span className={styles.quickLinkText}>NuGet Package</span>
            </a>
            <a 
              href="https://github.com/yourusername/easyvalidate/issues" 
              className={styles.quickLink}
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className={styles.quickLinkIcon}>🐛</span>
              <span className={styles.quickLinkText}>Report Issues</span>
            </a>
            <a 
              href="https://github.com/yourusername/easyvalidate/discussions" 
              className={styles.quickLink}
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className={styles.quickLinkIcon}>💬</span>
              <span className={styles.quickLinkText}>Community</span>
            </a>
          </div>
        </div>
      </div>
    </section>
  );
}

export default NextStepsSection;
