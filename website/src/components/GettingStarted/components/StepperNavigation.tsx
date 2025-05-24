import type { Step } from './StepData';
import styles from '../GettingStarted.module.css';

interface StepperNavigationProps {
  steps: Step[];
  currentStep: number;
  onStepClick: (stepIndex: number) => void;
}

function StepperNavigation({ steps, currentStep, onStepClick }: StepperNavigationProps) {
  return (
    <div className={styles.stepperNav}>
      {steps.map((step, index) => (
        <div
          key={step.id}
          className={`${styles.stepperItem} ${
            index === currentStep ? styles.active : ''
          } ${index < currentStep ? styles.completed : ''}`}
          onClick={() => onStepClick(index)}
        >
          <div className={styles.stepperIcon}>
            <span className={styles.stepperEmoji}>{step.icon}</span>
            <span className={styles.stepperNumber}>{index + 1}</span>
          </div>
          <div className={styles.stepperContent}>
            <h4 className={styles.stepperTitle}>{step.title}</h4>
            <p className={styles.stepperDescription}>{step.description}</p>
          </div>
          {index < steps.length - 1 && <div className={styles.stepperLine} />}
        </div>
      ))}
    </div>
  );
}

export default StepperNavigation;
