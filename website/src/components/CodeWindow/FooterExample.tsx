import React from 'react';
import CodeWindow from './CodeWindow';

const FooterExample: React.FC = () => {
  const exampleCode = [
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

// Usage
var user = new User { Name = "John", Email = "john@example.com", Age = 25 };
var result = user.Validate();

Console.WriteLine(result.IsValid ? "✅ Valid" : "❌ Invalid");`,
      active: true
    }
  ];

  const footerContent = (
    <div style={{ 
      display: 'flex', 
      alignItems: 'center', 
      justifyContent: 'space-between',
      width: '100%'
    }}>
      <span style={{ 
        color: '#9ca3af', 
        fontSize: '0.8rem' 
      }}>
        💡 This example demonstrates basic validation with EasyValidate
      </span>
      <div style={{ 
        display: 'flex', 
        gap: '0.5rem' 
      }}>
        <a 
          href="#docs" 
          style={{ 
            color: '#569CD6', 
            fontSize: '0.75rem',
            textDecoration: 'none'
          }}
        >
          📖 Learn More
        </a>
        <button style={{ 
          background: 'rgba(16, 185, 129, 0.1)',
          border: '1px solid #10b981',
          color: '#10b981',
          padding: '0.4rem 0.8rem',
          borderRadius: '4px',
          fontSize: '0.75rem',
          cursor: 'pointer'
        }}>
          Try Example
        </button>
      </div>
    </div>
  );

  return (
    <div style={{ padding: '2rem' }}>
      <h2>CodeWindow with Footer Example</h2>
      <p>This demonstrates the optional footer feature of the CodeWindow component:</p>
      
      <CodeWindow 
        windows={exampleCode}
        variant="light"
        footer={footerContent}
      />
      
      <div style={{ marginTop: '2rem', fontSize: '0.9rem', color: '#666' }}>
        <h3>Features demonstrated:</h3>
        <ul>
          <li>✅ Optional footer prop accepts any React node</li>
          <li>✅ Footer appears below the code content</li>
          <li>✅ Styled with proper contrast and spacing</li>
          <li>✅ Responsive design for mobile devices</li>
          <li>✅ Can contain text, links, buttons, or any React content</li>
        </ul>
      </div>
    </div>
  );
};

export default FooterExample;
