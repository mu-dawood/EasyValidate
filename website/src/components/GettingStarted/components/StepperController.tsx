import { useState } from 'react';
import CodeWindow from '../../CodeWindow/CodeWindow';
import styles from './StepperController.module.css';

// Fresh data for interactive demo
const demoScenarios = [
    {
        id: 'basic',
        title: 'Basic Validation',
        icon: '⚡',
        description: 'Simple model validation with attributes',
        color: '#3b82f6',
        code: [
            {
                fileName: 'UserModel.cs',
                language: 'csharp',
                snipt: `using EasyValidate.Attributes;

public partial class User
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    [Email]
    public string Email { get; set; }

    [Range(18, 120)]
    public int Age { get; set; }
}
`
            }, {
                fileName: 'ValidationResult.cs',
                language: 'csharp',
                snipt: `
// Usage
var user = new User { Name = "John", Email = "john@example.com", Age = 25 };
var result = user.Validate();

Console.WriteLine(result.IsValid ? "✅ Valid" : "❌ Invalid");
                `
            }
        ]
    },
    {
        id: 'collections',
        title: 'Collections & Nested',
        icon: '🔗',
        description: 'Validate collections and nested objects',
        color: '#10b981',
        code: [
            {
                fileName: 'ProductModel.cs',
                language: 'csharp',
                snipt: `public partial class Product
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    [Range(0.01, 10000)]
    public decimal Price { get; set; }

    // Nested validation
    [Required]
    public Category Category { get; set; }

    // Collection validation
    [CollectionNotEmpty]
    public List<string> Tags { get; set; }
}`
            }, {
                fileName: 'CategoryModel.cs',
                language: 'csharp',
                snipt: `
public partial class Category
{
    [Required]
    public string Name { get; set; }
}`
            }
        ]
    },
    {
        id: 'custom',
        title: 'Custom Rules',
        icon: '🎨',
        description: 'Create your own validation logic',
        color: '#8b5cf6',
        code: [
            {
                fileName: 'CustomValidation.cs',
                language: 'csharp',
                snipt: `public partial class Account
{
    [Required]
    public string Username { get; set; }

    [Password(MinLength = 8, RequireDigits = true)]
    public string Password { get; set; }

    // Custom validation method
    public ValidationResult CustomValidate()
    {
        if (Username?.ToLower() == "admin")
            return ValidationResult.Error("Username 'admin' is reserved");
        
        return ValidationResult.Success();
    }
}`
            }
        ]
    }
];

function StepperController() {
    const [activeDemo, setActiveDemo] = useState('basic');

    const currentDemo = demoScenarios.find(demo => demo.id === activeDemo) || demoScenarios[0];

    return (
        <div className={styles.interactiveDemo}>
            <div className={styles.demoHeader}>
                <h3 className={styles.demoTitle}>Try It Yourself</h3>
                <p className={styles.demoSubtitle}>Interactive examples to get you started</p>
            </div>

            <div className={styles.demoContainer}>
                {/* Left: Demo Cards */}
                <div className={styles.demoCards}>
                    {demoScenarios.map((demo) => (
                        <div
                            key={demo.id}
                            className={`${styles.demoCard} ${activeDemo === demo.id ? styles.active : ''}`}
                            onClick={() => setActiveDemo(demo.id)}
                            style={{ '--accent-color': demo.color } as React.CSSProperties}
                        >
                            <div className={styles.cardIcon}>
                                <span>{demo.icon}</span>
                            </div>
                            <div className={styles.cardContent}>
                                <h4 className={styles.cardTitle}>{demo.title}</h4>
                                <p className={styles.cardDescription}>{demo.description}</p>
                            </div>
                            <div className={styles.cardArrow}>→</div>
                        </div>
                    ))}
                </div>

                {/* Right: Code Display */}
                <CodeWindow
                    key={currentDemo.id}
                    windows={currentDemo.code.map(code => ({
                        fileName: code.fileName,
                        language: code.language,
                        snipt: code.snipt,
                    }))}
                    footer={(
                        <div className={styles.footerContent}>
                            <span className={styles.footerText}>
                                <span className={styles.tipIcon}>💡</span>
                                Click the cards on the left to explore different validation scenarios
                            </span>
                        </div>
                    )}

                />
            </div>
        </div>
    );
}

export default StepperController;
