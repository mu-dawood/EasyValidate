import { useState, useEffect } from 'react';
import clsx from 'clsx';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import styles from './CodeWindow.module.css';

// Window component for multi-tab support
export type Window = {
    fileName: string;
    snipt: string;
    language: string;
    active?: boolean;
    header?: React.ReactNode;
    footer?: React.ReactNode;
}

export type MultiWindowProps = {
    windows: Window[];
    className?: string;
    showCopyButton?: boolean;
    variant?: 'default' | 'hero' | 'colorful' | 'light';
    footer?: React.ReactNode;
    header?: React.ReactNode;
};

function CodeWindow({
    windows,
    className = '',
    showCopyButton = true,
    variant = 'light',
    footer, header
}: MultiWindowProps) {

    // Find initial active tab (if specified)
    const initialActiveIndex = windows.findIndex(window => window.active);

    const [activeTabIndex, setActiveTabIndex] = useState(initialActiveIndex >= 0 ? initialActiveIndex : 0);
    const [copied, setCopied] = useState(false);

    // Get the active tab
    const activeTab = windows[activeTabIndex] || windows[0];

    const copyToClipboard = () => {
        // Get text content from active tab's snipt
        const textContent = activeTab.snipt;

        navigator.clipboard.writeText(textContent)
            .then(() => {
                setCopied(true);
                setTimeout(() => setCopied(false), 2000);
            })
            .catch(err => {
                console.error('Failed to copy code: ', err);
            });
    };

    const CopyIcon = () => (
        <svg className={styles.btnIcon} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <rect x="9" y="9" width="13" height="13" rx="2" ry="2"></rect>
            <path d="M5 15H4a2 2 0 01-2-2V4a2 2 0 012-2h9a2 2 0 012 2v1"></path>
        </svg>
    );

    const CheckIcon = () => (
        <svg className={styles.btnIcon} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
            <polyline points="20 6 9 17 4 12"></polyline>
        </svg>
    );

    const getFileIcon = (name: string): string => {
        if (name.endsWith('.cs')) return '🔷';
        if (name.endsWith('.js') || name.endsWith('.ts')) return '🟡';
        if (name.endsWith('.json')) return '📄';
        if (name.includes('Console')) return '💻';
        if (name.endsWith('.xml') || name.endsWith('.csproj')) return '🧩';
        if (name.endsWith('.md')) return '📝';
        return '📄';
    };

    useEffect(() => {
        Prism.highlightAll();
    }, [activeTab.snipt, activeTab.language]);

    // Check if there's any footer (individual window footer or global footer)
    const hasFooter = activeTab.footer || footer;

    return (
        <div className={clsx(
            styles.codeWindow,
            styles[variant],
            className,
            { [styles.hasFooter]: hasFooter }
        )} style={{ overflow: 'auto', width: '100%' }}>
            <div className={styles.windowHeader}>
                {/* Display tabs if multiple tabs are available - positioned at start */}
                <div className={styles.tabsContainer}>
                    {windows.map((window, index) => (
                        <button
                            key={index}
                            className={clsx(styles.tabButton, { [styles.activeTab]: activeTabIndex === index })}
                            onClick={() => setActiveTabIndex(index)}
                        >
                            <span className={styles.fileIcon}>{getFileIcon(window.fileName)}</span>
                            <span className={styles.tabName}>{window.fileName}</span>
                            {showCopyButton && activeTabIndex === index && (
                                <button
                                    className={clsx(styles.tabCopyBtn, { [styles.copySuccess]: copied })}
                                    title={copied ? "Copied!" : "Copy code"}
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        copyToClipboard();
                                    }}
                                    aria-label="Copy code to clipboard"
                                >
                                    {copied ? <CheckIcon /> : <CopyIcon />}
                                </button>
                            )}
                        </button>
                    ))}
                </div>

                {/* Window actions positioned at end (right) */}
                <div className={styles.windowActions}>
                    {/* Any additional controls can be placed here */}
                </div>

                {/* Window controls moved to end and reversed */}
                <div className={styles.windowControls}>
                    <span className={clsx(styles.control, styles.controlMaximize)}></span>
                    <span className={clsx(styles.control, styles.controlMinimize)}></span>
                    <span className={clsx(styles.control, styles.controlClose)}></span>
                </div>
            </div>

            <div className={styles.mobileTabsNav}>
                <div className={styles.mobileTabsScroll}>
                    {windows.map((window, index) => (
                        <button
                            key={index}
                            className={clsx(styles.mobileTabButton, { [styles.activeTab]: activeTabIndex === index })}
                            onClick={() => setActiveTabIndex(index)}
                        >
                            <span>{getFileIcon(window.fileName)}</span> {window.fileName}
                        </button>
                    ))}
                </div>
            </div>

            {/* Dropdown selector for very small screens */}
            <div className={styles.tabSelector}>
                <select
                    className={styles.tabSelect}
                    value={activeTabIndex}
                    onChange={(e) => setActiveTabIndex(Number(e.target.value))}
                    aria-label="Select file tab"
                >
                    {windows.map((window, index) => (
                        <option key={index} value={index}>
                            {getFileIcon(window.fileName)} {window.fileName}
                        </option>
                    ))}
                </select>
            </div>
            {/* Optional Global Header */}
            {header && (
                <div className={styles.individualWindowHeader}>
                    {header}
                </div>
            )}
            {/* Optional Window Header (per individual window) */}
            {activeTab.header && (
                <div className={styles.individualWindowHeader}>
                    {activeTab.header}
                </div>
            )}

            <div className={styles.codeContent}>
                <pre className={styles.pre}>
                    <code className={`language-${activeTab.language}`}>
                        {activeTab.snipt}
                    </code>
                </pre>
            </div>

            {/* Optional Window Footer (per individual window) */}
            {activeTab.footer && (
                <div className={styles.codeFooter}>
                    {activeTab.footer}
                </div>
            )}

            {/* Optional Global Footer */}
            {footer && (
                <div className={styles.codeFooter}>
                    {footer}
                </div>
            )}
        </div>
    );
}

export default CodeWindow;