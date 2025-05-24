import React from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './AttributeSection.module.css';

const CollectionAttributes: React.FC = () => {
  return (
    <div className={styles.section}>
      <h2 className={styles.sectionTitle}>Collection Validation Attributes</h2>
      <p className={styles.sectionDescription}>
        Comprehensive validation attributes for arrays, lists, and other collection types.
      </p>

      <div className={styles.attributeGrid}>
        {/* CollectionNotEmpty Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>CollectionNotEmpty</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection is not null and contains at least one element.
          </p>
          <InlineSnippet
            snipt={`[CollectionNotEmpty]
public List<string> Tags { get; set; }`}
            language="csharp"
          />
        </div>

        {/* CollectionMinLength Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>CollectionMinLength</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection contains at least a minimum number of elements.
          </p>
          <InlineSnippet
            snipt={`[CollectionMinLength(3)]
public string[] RequiredItems { get; set; }`}
            language="csharp"
          />
        </div>

        {/* CollectionMaxLength Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>CollectionMaxLength</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection contains at most a maximum number of elements.
          </p>
          <InlineSnippet
            snipt={`[CollectionMaxLength(10)]
public List<int> LimitedList { get; set; }`}
            language="csharp"
          />
        </div>

        {/* CollectionExactLength Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>CollectionExactLength</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection contains exactly a specified number of elements.
          </p>
          <InlineSnippet
            snipt={`[CollectionExactLength(5)]
public int[] FixedArray { get; set; }`}
            language="csharp"
          />
        </div>

        {/* CollectionRange Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>CollectionRange</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection length falls within a specified range.
          </p>
          <InlineSnippet
            snipt={`[CollectionRange(2, 8)]
public List<string> FlexibleList { get; set; }`}
            language="csharp"
          />
        </div>

        {/* UniqueElements Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>UniqueElements</h3>
          <p className={styles.attributeDescription}>
            Validates that all elements in a collection are unique.
          </p>
          <InlineSnippet
            snipt={`[UniqueElements]
public List<int> UniqueNumbers { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NoNullElements Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NoNullElements</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection contains no null elements.
          </p>
          <InlineSnippet
            snipt={`[NoNullElements]
public List<object> ValidObjects { get; set; }`}
            language="csharp"
          />
        </div>

        {/* AllElementsValid Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>AllElementsValid</h3>
          <p className={styles.attributeDescription}>
            Validates that all elements in a collection pass validation.
          </p>
          <InlineSnippet
            snipt={`[AllElementsValid]
public List<EmailModel> EmailList { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ContainsElement Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ContainsElement</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection contains a specific element.
          </p>
          <InlineSnippet
            snipt={`[ContainsElement("Required")]
public List<string> MustHaveList { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotContainsElement Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotContainsElement</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection does not contain a specific element.
          </p>
          <InlineSnippet
            snipt={`[NotContainsElement("Forbidden")]
public List<string> CleanList { get; set; }`}
            language="csharp"
          />
        </div>

        {/* SortedAscending Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>SortedAscending</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection is sorted in ascending order.
          </p>
          <InlineSnippet
            snipt={`[SortedAscending]
public int[] AscendingNumbers { get; set; }`}
            language="csharp"
          />
        </div>

        {/* SortedDescending Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>SortedDescending</h3>
          <p className={styles.attributeDescription}>
            Validates that a collection is sorted in descending order.
          </p>
          <InlineSnippet
            snipt={`[SortedDescending]
public double[] DescendingValues { get; set; }`}
            language="csharp"
          />
        </div>
      </div>
    </div>
  );
};

export default CollectionAttributes;
