import styles from '../GettingStarted.module.css';

function FeaturesHighlight() {
  const features = [
    {
      icon: "⚡",
      title: "Zero Reflection",
      description: "Compile-time code generation for maximum performance"
    },
    {
      icon: "🔧",
      title: "IntelliSense Support",
      description: "Full IDE integration with error detection"
    },
    {
      icon: "📦",
      title: "Rich Attribute Library",
      description: "30+ built-in validation attributes"
    },
    {
      icon: "🌍",
      title: "Localization Ready",
      description: "Multi-language error messages out of the box"
    }
  ];

  return (
    <div className={styles.featuresHighlight}>
      <h3 className={styles.featuresTitle}>Why Developers Love EasyValidate</h3>
      <div className={styles.featuresList}>
        {features.map((feature, index) => (
          <div key={index} className={styles.feature}>
            <span className={styles.featureIcon}>{feature.icon}</span>
            <div>
              <strong>{feature.title}</strong>
              <span>{feature.description}</span>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default FeaturesHighlight;
