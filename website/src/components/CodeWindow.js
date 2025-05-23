import React, { useState, useEffect } from 'react';
import clsx from 'clsx';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import styles from './CodeWindow.module.css';

function CodeWindow({
    fileName = "code.cs",
    children,
    language = 'csharp',
    className = '',
    showCopyButton = true,
    variant = 'light' // 'default', 'hero', 'colorful', 'light'
}) {
    const [copied, setCopied] = useState(false);

    const copyToClipboard = () => {
        // Extract text content from children
        const textContent = typeof children === 'string' ? children : children?.props?.children;

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

    const getFileIcon = () => {
        if (fileName.endsWith('.cs')) return '🔷';
        if (fileName.endsWith('.js') || fileName.endsWith('.ts')) return '🟡';
        if (fileName.endsWith('.json')) return '📄';
        if (fileName.includes('Console')) return '💻';
        if (fileName.endsWith('.xml') || fileName.endsWith('.csproj')) return '🧩';
        if (fileName.endsWith('.md')) return '📝';
        return '📄';
    };

    useEffect(() => {
        Prism.highlightAll();
    }, [children, language]);

    return (
        <div className={clsx(styles.codeWindow, styles[variant], className)} style={{ overflow: 'auto', width: '100%' }}>
            <div className={styles.windowHeader}>
                <div className={styles.windowControls}>
                    <span className={clsx(styles.control, styles.controlClose)}></span>
                    <span className={clsx(styles.control, styles.controlMinimize)}></span>
                    <span className={clsx(styles.control, styles.controlMaximize)}></span>
                </div>
                <div className={styles.windowTitle}>
                    <span className={styles.fileIcon}>{getFileIcon()}</span>
                    <span className={styles.fileName}>{fileName}</span>
                </div>
                <div className={styles.windowActions}>
                    {showCopyButton && (
                        <button
                            className={clsx(styles.copyBtn, { [styles.copySuccess]: copied })}
                            title={copied ? "Copied!" : "Copy code"}
                            onClick={copyToClipboard}
                            aria-label="Copy code to clipboard"
                        >
                            {copied ? <><CheckIcon /> Copied!</> : <><CopyIcon /> Copy</>}
                        </button>
                    )}
                </div>
            </div>
            <div className={styles.codeContent} style={{ overflowY: 'auto', maxHeight: '400px' }}>
                <pre className={styles.pre}>
                    <code className={`language-${language}`}>
                        {typeof children === 'string' ? children : children?.props?.children || ''}
                    </code>
                </pre>
            </div>
        </div>
    );
}

export default CodeWindow;
