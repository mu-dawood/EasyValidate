import React from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './AttributeSection.module.css';

const StringAttributes: React.FC = () => {
  return (
    <div className={styles.section}>
      <h2 className={styles.sectionTitle}>String Validation Attributes</h2>
      <p className={styles.sectionDescription}>
        Comprehensive string validation attributes for text input validation.
      </p>

      <div className={styles.attributeGrid}>
        {/* Required Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Required</h3>
          <p className={styles.attributeDescription}>
            Ensures that a string property is not null, empty, or whitespace.
          </p>
          <InlineSnippet
            snipt={`[Required]
public string Name { get; set; }`}
            language="csharp"
          />
        </div>

        {/* StringLength Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>StringLength</h3>
          <p className={styles.attributeDescription}>
            Validates that a string property has a length within specified bounds.
          </p>
          <InlineSnippet
            snipt={`[StringLength(50, MinimumLength = 2)]
public string Description { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MinLength Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MinLength</h3>
          <p className={styles.attributeDescription}>
            Specifies the minimum length allowed for a string property.
          </p>
          <InlineSnippet
            snipt={`[MinLength(3)]
public string Username { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MaxLength Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MaxLength</h3>
          <p className={styles.attributeDescription}>
            Specifies the maximum length allowed for a string property.
          </p>
          <InlineSnippet
            snipt={`[MaxLength(100)]
public string Title { get; set; }`}
            language="csharp"
          />
        </div>

        {/* EmailAddress Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>EmailAddress</h3>
          <p className={styles.attributeDescription}>
            Validates that a string property contains a valid email address format.
          </p>
          <InlineSnippet
            snipt={`[EmailAddress]
public string Email { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Phone Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Phone</h3>
          <p className={styles.attributeDescription}>
            Validates that a string property contains a valid phone number format.
          </p>
          <InlineSnippet
            snipt={`[Phone]
public string PhoneNumber { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Url Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Url</h3>
          <p className={styles.attributeDescription}>
            Validates that a string property contains a valid URL format.
          </p>
          <InlineSnippet
            snipt={`[Url]
public string Website { get; set; }`}
            language="csharp"
          />
        </div>

        {/* RegularExpression Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>RegularExpression</h3>
          <p className={styles.attributeDescription}>
            Validates that a string property matches a specified regular expression pattern.
          </p>
          <InlineSnippet
            snipt={`[RegularExpression(@"^[a-zA-Z0-9]*$")]
public string AlphaNumericCode { get; set; }`}
            language="csharp"
          />
        </div>

        {/* CreditCard Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>CreditCard</h3>
          <p className={styles.attributeDescription}>
            Validates that a string property contains a valid credit card number format.
          </p>
          <InlineSnippet
            snipt={`[CreditCard]
public string CardNumber { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Password Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Password</h3>
          <p className={styles.attributeDescription}>
            Validates password complexity requirements including special characters.
          </p>
          <InlineSnippet
            snipt={`[Password]
public string Password { get; set; }`}
            language="csharp"
          />
        </div>

        {/* AlphaNumeric Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>AlphaNumeric</h3>
          <p className={styles.attributeDescription}>
            Validates that a string contains only alphanumeric characters.
          </p>
          <InlineSnippet
            snipt={`[AlphaNumeric]
public string Code { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NoWhitespace Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NoWhitespace</h3>
          <p className={styles.attributeDescription}>
            Validates that a string does not contain any whitespace characters.
          </p>
          <InlineSnippet
            snipt={`[NoWhitespace]
public string Identifier { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ContainsUpper Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ContainsUpper</h3>
          <p className={styles.attributeDescription}>
            Validates that a string contains at least one uppercase letter.
          </p>
          <InlineSnippet
            snipt={`[ContainsUpper]
public string StrongPassword { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ContainsLower Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ContainsLower</h3>
          <p className={styles.attributeDescription}>
            Validates that a string contains at least one lowercase letter.
          </p>
          <InlineSnippet
            snipt={`[ContainsLower]
public string ComplexPassword { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ContainsDigit Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ContainsDigit</h3>
          <p className={styles.attributeDescription}>
            Validates that a string contains at least one numeric digit.
          </p>
          <InlineSnippet
            snipt={`[ContainsDigit]
public string SecureCode { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ContainsSpecial Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ContainsSpecial</h3>
          <p className={styles.attributeDescription}>
            Validates that a string contains at least one special character.
          </p>
          <InlineSnippet
            snipt={`[ContainsSpecial]
public string SecurePassword { get; set; }`}
            language="csharp"
          />
        </div>

        {/* FirstLetterUpper Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>FirstLetterUpper</h3>
          <p className={styles.attributeDescription}>
            Validates that the first letter of a string is uppercase.
          </p>
          <InlineSnippet
            snipt={`[FirstLetterUpper]
public string ProperName { get; set; }`}
            language="csharp"
          />
        </div>

        {/* JsonFormat Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>JsonFormat</h3>
          <p className={styles.attributeDescription}>
            Validates that a string contains valid JSON format.
          </p>
          <InlineSnippet
            snipt={`[JsonFormat]
public string Configuration { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Base64Format Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Base64Format</h3>
          <p className={styles.attributeDescription}>
            Validates that a string is properly formatted as Base64.
          </p>
          <InlineSnippet
            snipt={`[Base64Format]
public string EncodedData { get; set; }`}
            language="csharp"
          />
        </div>
      </div>
    </div>
  );
};

export default StringAttributes;
