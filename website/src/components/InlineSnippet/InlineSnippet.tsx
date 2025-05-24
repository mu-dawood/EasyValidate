import React, { useEffect } from 'react';
import Prism from 'prismjs';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import styles from './InlineSnippet.module.css';

type InlineSnippetProps = {
  snipt: React.ReactNode;
  language?: string;
  className?: string;
};

function InlineSnippet({ snipt, language = 'csharp', className = '' }: InlineSnippetProps) {
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
          {snipt}
        </code>
      </pre>
    </div>
  );
}

export default InlineSnippet;
