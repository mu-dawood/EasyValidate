import React from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './AttributeSection.module.css';

const DateAttributes: React.FC = () => {
  return (
    <div className={styles.section}>
      <h2 className={styles.sectionTitle}>Date & Time Validation Attributes</h2>
      <p className={styles.sectionDescription}>
        Comprehensive date and time validation attributes for temporal data validation.
      </p>

      <div className={styles.attributeGrid}>
        {/* FutureDate Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>FutureDate</h3>
          <p className={styles.attributeDescription}>
            Validates that a date value is in the future.
          </p>
          <InlineSnippet
            snipt={`[FutureDate]
public DateTime EventDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* PastDate Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>PastDate</h3>
          <p className={styles.attributeDescription}>
            Validates that a date value is in the past.
          </p>
          <InlineSnippet
            snipt={`[PastDate]
public DateTime BirthDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* TodayDate Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>TodayDate</h3>
          <p className={styles.attributeDescription}>
            Validates that a date value is today's date.
          </p>
          <InlineSnippet
            snipt={`[TodayDate]
public DateTime ProcessingDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotTodayDate Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotTodayDate</h3>
          <p className={styles.attributeDescription}>
            Validates that a date value is not today's date.
          </p>
          <InlineSnippet
            snipt={`[NotTodayDate]
public DateTime ScheduledDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* DateRange Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>DateRange</h3>
          <p className={styles.attributeDescription}>
            Validates that a date value falls within a specified date range.
          </p>
          <InlineSnippet
            snipt={`[DateRange("2020-01-01", "2030-12-31")]
public DateTime ValidPeriod { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MinAge Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MinAge</h3>
          <p className={styles.attributeDescription}>
            Validates that a birth date represents a minimum age.
          </p>
          <InlineSnippet
            snipt={`[MinAge(18)]
public DateTime DateOfBirth { get; set; }`}
            language="csharp"
          />
        </div>

        {/* MaxAge Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>MaxAge</h3>
          <p className={styles.attributeDescription}>
            Validates that a birth date represents a maximum age.
          </p>
          <InlineSnippet
            snipt={`[MaxAge(65)]
public DateTime EmployeeBirthDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* AgeRange Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>AgeRange</h3>
          <p className={styles.attributeDescription}>
            Validates that a birth date represents an age within a specified range.
          </p>
          <InlineSnippet
            snipt={`[AgeRange(21, 65)]
public DateTime WorkerBirthDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* WeekDay Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>WeekDay</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls on a weekday (Monday through Friday).
          </p>
          <InlineSnippet
            snipt={`[WeekDay]
public DateTime BusinessDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Weekend Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Weekend</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls on a weekend (Saturday or Sunday).
          </p>
          <InlineSnippet
            snipt={`[Weekend]
public DateTime LeisureDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* SpecificDay Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>SpecificDay</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls on a specific day of the week.
          </p>
          <InlineSnippet
            snipt={`[SpecificDay(DayOfWeek.Monday)]
public DateTime MondayMeeting { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotSpecificDay Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotSpecificDay</h3>
          <p className={styles.attributeDescription}>
            Validates that a date does not fall on a specific day of the week.
          </p>
          <InlineSnippet
            snipt={`[NotSpecificDay(DayOfWeek.Sunday)]
public DateTime WorkDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Quarter Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Quarter</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls within a specific quarter of the year.
          </p>
          <InlineSnippet
            snipt={`[Quarter(1)]
public DateTime FirstQuarterDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Month Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Month</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls within a specific month.
          </p>
          <InlineSnippet
            snipt={`[Month(12)]
public DateTime ChristmasEvent { get; set; }`}
            language="csharp"
          />
        </div>

        {/* Year Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>Year</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls within a specific year.
          </p>
          <InlineSnippet
            snipt={`[Year(2024)]
public DateTime CurrentYearEvent { get; set; }`}
            language="csharp"
          />
        </div>

        {/* LeapYear Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>LeapYear</h3>
          <p className={styles.attributeDescription}>
            Validates that a date falls within a leap year.
          </p>
          <InlineSnippet
            snipt={`[LeapYear]
public DateTime LeapYearDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotLeapYear Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotLeapYear</h3>
          <p className={styles.attributeDescription}>
            Validates that a date does not fall within a leap year.
          </p>
          <InlineSnippet
            snipt={`[NotLeapYear]
public DateTime RegularYearDate { get; set; }`}
            language="csharp"
          />
        </div>

        {/* TimeRange Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>TimeRange</h3>
          <p className={styles.attributeDescription}>
            Validates that a time value falls within a specified time range.
          </p>
          <InlineSnippet
            snipt={`[TimeRange("09:00", "17:00")]
public TimeSpan BusinessHours { get; set; }`}
            language="csharp"
          />
        </div>

        {/* UTC Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>UTC</h3>
          <p className={styles.attributeDescription}>
            Validates that a DateTime value is in UTC timezone.
          </p>
          <InlineSnippet
            snipt={`[UTC]
public DateTime UtcTimestamp { get; set; }`}
            language="csharp"
          />
        </div>

        {/* NotUTC Attribute */}
        <div className={styles.attributeCard}>
          <h3 className={styles.attributeTitle}>NotUTC</h3>
          <p className={styles.attributeDescription}>
            Validates that a DateTime value is not in UTC timezone.
          </p>
          <InlineSnippet
            snipt={`[NotUTC]
public DateTime LocalTime { get; set; }`}
            language="csharp"
          />
        </div>
      </div>
    </div>
  );
};

export default DateAttributes;
