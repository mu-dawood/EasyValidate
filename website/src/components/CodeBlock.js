import React, { useEffect } from 'react';
import clsx from 'clsx';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import styles from './CodeBlock.module.css';

function CodeBlock({ children, language = 'csharp', className = '' }) {
  useEffect(() => {
    Prism.highlightAll();
  }, []);

  return (
    <div className={clsx(styles.codeBlock, className)}>
      <pre>
        <code className={`language-${language}`}>
          {children}
        </code>
      </pre>
    </div>
  );
}

export default CodeBlock;
