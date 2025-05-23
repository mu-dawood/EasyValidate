import React from 'react';
import Layout from '@theme/Layout';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import HeroSection from '../components/HeroSection';
import CodeBlock from '../components/CodeBlock';
import HowItWorksSection from '../components/HowItWorksSection';
import AttributesSection from '../components/AttributesSection';
import QuickStartSection from '../components/QuickStartSection';
import CallToActionSection from '../components/CallToActionSection';
import styles from './index.module.css';

// Updated Home component to use the new components
export default function Home() {
  const { siteConfig } = useDocusaurusContext();
  return (
    <Layout
      title={`${siteConfig.title} - Source Generator for .NET Validation`}
      description="Source Generator for Attribute-Based Validation in .NET. Automatically generates validation methods from attributes at compile-time.">
      <HeroSection />
      <main>
        <HowItWorksSection />
        <AttributesSection />
        <QuickStartSection />
        <CallToActionSection />
      </main>
    </Layout>
  );
}