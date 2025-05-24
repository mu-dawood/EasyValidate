import React from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './AttributeSection.module.css';

const NumericAttributes: React.FC = () => {
  return (
    <div className={styles.section}>
      <h2 className={styles.sectionTitle}>Numeric Validation Attributes</h2>
      <p className={styles.sectionDescription}>
        Comprehensive numeric validation attributes for integer, decimal, and floating-point values.
      </p>

      <div className={styles.attributeGrid}>
        {/* Range Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Range</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is within a specified range.
          </p>
          <InlineSnippet
            snipt={`[Range(1, 100)]
public int Age { get; set; }`}
            language="csharp"
          />
        </div>

        {/* GreaterThan Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>GreaterThan</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is greater than a specified minimum.
          </p>
          <InlineSnippet
            snipt={`[GreaterThan(0)]
public decimal Price { get; set; }`}
            language="csharp"
          />
        </div>

        {/* LessThan Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>LessThan</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is less than a specified maximum.
          </p>
          <InlineSnippet
            snipt={`[LessThan(100)]
public int Percentage { get; set; }`}
            language="csharp"
          />
        </div>

        {/* GreaterThanOrEqual Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>GreaterThanOrEqual</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is greater than or equal to a specified minimum.
          </p>
          <InlineSnippet
            snipt={`[GreaterThanOrEqual(18)]
public int MinimumAge { get; set; }`}
            language="csharp"
          />
        </div>

        {/* LessThanOrEqual Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>LessThanOrEqual</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is less than or equal to a specified maximum.
          </p>
          <InlineSnippet
            snipt={`[LessThanOrEqual(65)]
public int RetirementAge { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Positive Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Positive</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is positive (greater than zero).
          </p>
          <InlineSnippet
            snipt={`[Positive]
public double Amount { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Negative Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Negative</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is negative (less than zero).
          </p>
          <InlineSnippet
            snipt={`[Negative]
public decimal Debt { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Zero Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Zero</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is exactly zero.
          </p>
          <InlineSnippet
            snipt={`[Zero]
public int ResetValue { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NonZero Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NonZero</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is not zero.
          </p>
          <InlineSnippet
            snipt={`[NonZero]
public int Divisor { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Even Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Even</h3>
          <p className={styles.attributeDescription}>
            Validates that an integer value is even.
          </p>
          <InlineSnippet
            snipt={`[Even]
public int EvenNumber { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Odd Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Odd</h3>
          <p className={styles.attributeDescription}>
            Validates that an integer value is odd.
          </p>
          <InlineSnippet
            snipt={`[Odd]
public int OddNumber { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Precision Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Precision</h3>
          <p className={styles.attributeDescription}>
            Validates the precision (total digits) and scale (decimal places) of a decimal value.
          </p>
          <InlineSnippet
            snipt={`[Precision(10, 2)]
public decimal Currency { get; set; }`}
            language="csharp"
          />
        </div>

        {/* DivisibleBy Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>DivisibleBy</h3>
          <p className={styles.attributeDescription}>
            Validates that a numeric value is divisible by a specified number.
          </p>
          <InlineSnippet
            snipt={`[DivisibleBy(5)]
public int MultipleOfFive { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Prime Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Prime</h3>
          <p className={styles.attributeDescription}>
            Validates that an integer value is a prime number.
          </p>
          <InlineSnippet
            snipt={`[Prime]
public int PrimeNumber { get; set; }`}
            language="csharp"
          />
        </div>

        {/* PowerOfTwo Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>PowerOfTwo</h3>
          <p className={styles.attributeDescription}>
            Validates that an integer value is a power of two.
          </p>
          <InlineSnippet
            snipt={`[PowerOfTwo]
public int BufferSize { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Fibonacci Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Fibonacci</h3>
          <p className={styles.attributeDescription}>
            Validates that an integer value is a Fibonacci number.
          </p>
          <InlineSnippet
            snipt={`[Fibonacci]
public int FibonacciValue { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotNaN Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotNaN</h3>
          <p className={styles.attributeDescription}>
            Validates that a floating-point value is not NaN (Not a Number).
          </p>
          <InlineSnippet
            snipt={`[NotNaN]
public double CalculatedValue { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotInfinity Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotInfinity</h3>
          <p className={styles.attributeDescription}>
            Validates that a floating-point value is not infinite.
          </p>
          <InlineSnippet
            snipt={`[NotInfinity]
public float Result { get; set; }`}
            language="csharp"
          />
        </div>

        {/* FiniteNumber Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>FiniteNumber</h3>
          <p className={styles.attributeDescription}>
            Validates that a floating-point value is finite (not NaN or infinite).
          </p>
          <InlineSnippet
            snipt={`[FiniteNumber]
public double MeasuredValue { get; set; }`}
            language="csharp"
          />
        </div>
      </div>
    </div>
  );
};

export default NumericAttributes;
