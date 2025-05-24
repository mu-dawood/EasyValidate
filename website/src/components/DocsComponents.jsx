import React from 'react';
import Layout from '@theme/Layout';
import FeatureCard from '../components/FeatureCard';
import InfoBox from '../components/InfoBox';
import DocSection from '../components/DocSection';
import InlineSnippet from '../components/InlineSnippet';
import AttributeCard from '../components/AttributeCard';
import CodeWindow from '../components/CodeWindow';

export function FeatureGrid({ children }) {
    return (
        <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
            gap: '1.5rem',
            marginTop: '2rem'
        }}>
            {children}
        </div>
    );
}

export function DocsWrapper({ children }) {
    return (
        <div className="docs-enhanced-styling">
            {children}
        </div>
    );
}

export { FeatureCard, InfoBox, DocSection, InlineSnippet, AttributeCard, CodeWindow };
