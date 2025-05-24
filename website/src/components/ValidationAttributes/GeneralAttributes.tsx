import React from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './AttributeSection.module.css';

const GeneralAttributes: React.FC = () => {
  return (
    <div className={styles.section}>
      <h2 className={styles.sectionTitle}>General Validation Attributes</h2>
      <p className={styles.sectionDescription}>
        General-purpose validation attributes for various data types and custom validation scenarios.
      </p>

      <div className={styles.attributeGrid}>
        {/* NotNull Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotNull</h3>
          <p className={styles.attributeDescription}>
            Validates that a value is not null.
          </p>
          <InlineSnippet
            snipt={`[NotNull]
public object RequiredObject { get; set; }`}
            language="csharp"
          />
        </div>

        {/* AllowNull Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>AllowNull</h3>
          <p className={styles.attributeDescription}>
            Explicitly allows null values for a property.
          </p>
          <InlineSnippet
            snipt={`[AllowNull]
public string OptionalValue { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotDefault Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotDefault</h3>
          <p className={styles.attributeDescription}>
            Validates that a value is not the default value for its type.
          </p>
          <InlineSnippet
            snipt={`[NotDefault]
public Guid UniqueId { get; set; }`}
            language="csharp"
          />
        </div>

        {/* EqualTo Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>EqualTo</h3>
          <p className={styles.attributeDescription}>
            Validates that a value is equal to another property's value.
          </p>
          <InlineSnippet
            snipt={`[EqualTo(nameof(Password))]
public string ConfirmPassword { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotEqualTo Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotEqualTo</h3>
          <p className={styles.attributeDescription}>
            Validates that a value is not equal to another property's value.
          </p>
          <InlineSnippet
            snipt={`[NotEqualTo(nameof(OldPassword))]
public string NewPassword { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MustBeTrue Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MustBeTrue</h3>
          <p className={styles.attributeDescription}>
            Validates that a boolean value is true.
          </p>
          <InlineSnippet
            snipt={`[MustBeTrue]
public bool AcceptTerms { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MustBeFalse Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MustBeFalse</h3>
          <p className={styles.attributeDescription}>
            Validates that a boolean value is false.
          </p>
          <InlineSnippet
            snipt={`[MustBeFalse]
public bool IsDeleted { get; set; }`}
            language="csharp"
          />
        </div>

        {/* OneOf Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>OneOf</h3>
          <p className={styles.attributeDescription}>
            Validates that a value is one of the specified allowed values.
          </p>
          <InlineSnippet
            snipt={`[OneOf("Active", "Inactive", "Pending")]
public string Status { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotOneOf Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotOneOf</h3>
          <p className={styles.attributeDescription}>
            Validates that a value is not one of the specified forbidden values.
          </p>
          <InlineSnippet
            snipt={`[NotOneOf("Admin", "Root", "System")]
public string Username { get; set; }`}
            language="csharp"
          />
        </div>

        {/* FileExists Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>FileExists</h3>
          <p className={styles.attributeDescription}>
            Validates that a file path points to an existing file.
          </p>
          <InlineSnippet
            snipt={`[FileExists]
public string ConfigFilePath { get; set; }`}
            language="csharp"
          />
        </div>

        {/* DirectoryExists Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>DirectoryExists</h3>
          <p className={styles.attributeDescription}>
            Validates that a directory path points to an existing directory.
          </p>
          <InlineSnippet
            snipt={`[DirectoryExists]
public string OutputDirectory { get; set; }`}
            language="csharp"
          />
        </div>

        {/* FileExtension Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>FileExtension</h3>
          <p className={styles.attributeDescription}>
            Validates that a file path has one of the specified extensions.
          </p>
          <InlineSnippet
            snipt={`[FileExtension(".jpg", ".png", ".gif")]
public string ImageFile { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ValidEnum Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ValidEnum</h3>
          <p className={styles.attributeDescription}>
            Validates that an enum value is defined in the enum type.
          </p>
          <InlineSnippet
            snipt={`[ValidEnum]
public UserRole Role { get; set; }`}
            language="csharp"
          />
        </div>

        {/* IPAddress Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>IPAddress</h3>
          <p className={styles.attributeDescription}>
            Validates that a string represents a valid IP address.
          </p>
          <InlineSnippet
            snipt={`[IPAddress]
public string ServerIP { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MacAddress Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MacAddress</h3>
          <p className={styles.attributeDescription}>
            Validates that a string represents a valid MAC address.
          </p>
          <InlineSnippet
            snipt={`[MacAddress]
public string NetworkCard { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Guid Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Guid</h3>
          <p className={styles.attributeDescription}>
            Validates that a string represents a valid GUID format.
          </p>
          <InlineSnippet
            snipt={`[Guid]
public string UniqueIdentifier { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Color Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Color</h3>
          <p className={styles.attributeDescription}>
            Validates that a string represents a valid color format (hex, rgb, etc.).
          </p>
          <InlineSnippet
            snipt={`[Color]
public string ThemeColor { get; set; }`}
            language="csharp"
          />
        </div>

        {/* PostalCode Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>PostalCode</h3>
          <p className={styles.attributeDescription}>
            Validates that a string represents a valid postal code format.
          </p>
          <InlineSnippet
            snipt={`[PostalCode]
public string ZipCode { get; set; }`}
            language="csharp"
          />
        </div>

        {/* ISBN Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>ISBN</h3>
          <p className={styles.attributeDescription}>
            Validates that a string represents a valid ISBN format.
          </p>
          <InlineSnippet
            snipt={`[ISBN]
public string BookIdentifier { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Custom Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Custom</h3>
          <p className={styles.attributeDescription}>
            Allows custom validation logic with a lambda expression or method.
          </p>
          <InlineSnippet
            snipt={`[Custom(x => x.Length > 5 && x.Contains("@"))]
public string CustomValidation { get; set; }`}
            language="csharp"
          />
        </div>
      </div>
    </div>
  );
};

export default GeneralAttributes;
