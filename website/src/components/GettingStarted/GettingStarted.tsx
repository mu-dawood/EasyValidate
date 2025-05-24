import {
  GettingStartedHero,
  StepperController,
  FeaturesHighlight,
  CallToAction
} from './components';
import styles from './GettingStarted.module.css';

function GettingStarted() {

  return (
    <section id="getting-started" className={styles.gettingStarted}>
      <div className={styles.container}>
        <GettingStartedHero />
        <StepperController />
        <FeaturesHighlight />
        <CallToAction />
      </div>
    </section>
  );
}

export default GettingStarted;
