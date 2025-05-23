import React, { useEffect } from 'react';
import clsx from 'clsx';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import styles from './InlineSnippet.module.css';

function InlineSnippet({ children, language = 'csharp', className = '' }) {
  useEffect(() => {
    Prism.highlightAll();
  }, []);

  return (
    <div className={clsx(styles.inlineSnippet, className)}>
      <pre>
        <code className={`language-${language}`}>
          {children}
        </code>
      </pre>
    </div>
  );
}

export default InlineSnippet;
