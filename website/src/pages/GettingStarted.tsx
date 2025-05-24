import {
  InstallationSection,
  QuickStartSection,
  FrameworkIntegrationSection,
  KeyFeaturesSection,
  NextStepsSection
} from '../components/GettingStartedComponents';
import styles from './GettingStarted.module.css';

function GettingStarted() {
  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Getting Started with EasyValidate</h1>
        <p className={styles.subtitle}>
          Master .NET validation in minutes with our comprehensive guide. From installation to advanced features, 
          everything you need to build robust, type-safe validation for your applications.
        </p>
      </div>

      <InstallationSection />
      <QuickStartSection />
      <FrameworkIntegrationSection />
      <KeyFeaturesSection />
      <NextStepsSection />
    </div>
  );
}

export default GettingStarted;
