import CodeWindow from '../../CodeWindow';
import type { Step } from './StepData';
import styles from '../GettingStarted.module.css';

interface StepContentProps {
  step: Step;
  stepIndex: number;
  totalSteps: number;
  onNextStep: () => void;
  onPrevStep: () => void;
}

function StepContent({ step, stepIndex, totalSteps, onNextStep, onPrevStep }: StepContentProps) {
  return (
    <div className={styles.stepContent}>
      <div className={styles.stepCard}>
        <div className={styles.stepHeader}>
          <div className={styles.stepNumber}>
            <span className={styles.stepIcon}>{step.icon}</span>
            <span className={styles.number}>{stepIndex + 1}</span>
          </div>
          <div className={styles.stepInfo}>
            <h3 className={styles.stepTitle}>{step.title}</h3>
            <p className={styles.stepDescription}>
              {step.description}
            </p>
          </div>
        </div>
        
        <div className={styles.stepCodeContent}>
          <CodeWindow windows={step.content.windows} variant="light" />
          
          {step.content.tip && (
            <div className={styles.stepTip}>
              <span className={styles.tipIcon}>💡</span>
              <span>{step.content.tip}</span>
            </div>
          )}
        </div>

        {/* Step Navigation */}
        <div className={styles.stepNavigation}>
          <button
            onClick={onPrevStep}
            disabled={stepIndex === 0}
            className={styles.navButton}
          >
            ← Previous
          </button>
          
          <div className={styles.stepIndicator}>
            Step {stepIndex + 1} of {totalSteps}
          </div>
          
          <button
            onClick={onNextStep}
            disabled={stepIndex === totalSteps - 1}
            className={styles.navButton}
          >
            Next →
          </button>
        </div>
      </div>
    </div>
  );
}

export default StepContent;
