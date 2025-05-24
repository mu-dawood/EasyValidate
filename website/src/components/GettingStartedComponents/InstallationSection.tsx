import { useState } from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './GettingStartedComponents.module.css';

interface InstallationMethod {
  id: string;
  name: string;
  icon: string;
  language: string;
  command: string;
  description: string;
}

function InstallationSection() {
  const [activeMethod, setActiveMethod] = useState('dotnet-cli');

  const installationMethods: InstallationMethod[] = [
    {
      id: 'dotnet-cli',
      name: '.NET CLI',
      icon: '🔧',
      language: 'bash',
      command: 'dotnet add package EasyValidate',
      description: 'Recommended for most .NET projects'
    },
    {
      id: 'package-manager',
      name: 'Package Manager Console',
      icon: '📦',
      language: 'powershell',
      command: 'Install-Package EasyValidate',
      description: 'Visual Studio Package Manager Console'
    },
    {
      id: 'package-reference',
      name: 'PackageReference',
      icon: '📄',
      language: 'xml',
      command: `<PackageReference Include="EasyValidate" Version="1.0.0" />`,
      description: 'Add directly to your .csproj file'
    },
    {
      id: 'nuget-cli',
      name: 'NuGet CLI',
      icon: '⚡',
      language: 'bash',
      command: 'nuget install EasyValidate',
      description: 'Command-line NuGet package manager'
    }
  ];

  const activeInstallation = installationMethods.find(method => method.id === activeMethod) || installationMethods[0];

  return (
    <section className={styles.section}>
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>
          <span className={styles.sectionIcon}>📦</span>
          Installation
        </h2>
        <p className={styles.sectionDescription}>
          Choose your preferred method to install EasyValidate in your .NET project
        </p>
      </div>

      <div className={styles.installationContainer}>
        {/* Installation Method Tabs */}
        <div className={styles.methodTabs}>
          {installationMethods.map((method) => (
            <button
              key={method.id}
              className={`${styles.methodTab} ${activeMethod === method.id ? styles.active : ''}`}
              onClick={() => setActiveMethod(method.id)}
            >
              <span className={styles.methodIcon}>{method.icon}</span>
              <span className={styles.methodName}>{method.name}</span>
            </button>
          ))}
        </div>

        {/* Active Installation Method */}
        <div className={styles.installationCard}>
          <div className={styles.installationHeader}>
            <div className={styles.installationTitle}>
              <span className={styles.installationIcon}>{activeInstallation.icon}</span>
              <h3>{activeInstallation.name}</h3>
            </div>
            <p className={styles.installationDescription}>
              {activeInstallation.description}
            </p>
          </div>

          <div className={styles.codeContainer}>
            <InlineSnippet 
              snipt={activeInstallation.command}
              language={activeInstallation.language}
              className={styles.installationCode}
            />
            <button 
              className={styles.copyButton}
              onClick={() => navigator.clipboard.writeText(activeInstallation.command)}
              title="Copy to clipboard"
            >
              📋
            </button>
          </div>
        </div>

        {/* Quick Tips */}
        <div className={styles.tipSection}>
          <div className={styles.tip}>
            <span className={styles.tipIcon}>💡</span>
            <div className={styles.tipContent}>
              <strong>Pro Tip:</strong> The .NET CLI method is recommended for most scenarios as it's cross-platform and works with all project types.
            </div>
          </div>
          <div className={styles.tip}>
            <span className={styles.tipIcon}>🎯</span>
            <div className={styles.tipContent}>
              <strong>Target Framework:</strong> EasyValidate supports .NET 6.0, .NET 7.0, .NET 8.0, and .NET Standard 2.0.
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}

export default InstallationSection;
