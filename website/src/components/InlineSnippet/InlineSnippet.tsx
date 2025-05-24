import React, { useEffect } from 'react';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import styles from './InlineSnippet.module.css';

type InlineSnippetProps = {
  children: React.ReactNode;
  language?: string;
  className?: string;
};

function InlineSnippet({ children, language = 'csharp', className = '' }: InlineSnippetProps) {
  useEffect(() => {
    Prism.highlightAll();
  }, []);

  const clsx = (...classes: (string | undefined | null | false)[]) => {
    return classes.filter(Boolean).join(' ');
  };

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
