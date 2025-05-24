import React, { useEffect, useRef } from 'react';
import Prism from 'prismjs';
import 'prismjs/themes/prism.css'; // Default light theme
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-bash';
import 'prismjs/components/prism-json';
import 'prismjs/components/prism-typescript';
import 'prismjs/components/prism-javascript';

type InlineSnippetProps = {
  snipt: React.ReactNode;
  language?: string;
  className?: string;
};

function InlineSnippet({ snipt, language = 'csharp', className = '' }: InlineSnippetProps) {
  const codeRef = useRef<HTMLElement>(null);

  useEffect(() => {
    if (codeRef.current) {
      // Highlight the specific code element
      Prism.highlightElement(codeRef.current);
    }
  }, [snipt, language]);

  return (
    <pre className={className}>
      <code 
        ref={codeRef}
        className={`language-${language}`}
      >
        {snipt}
      </code>
    </pre>
  );
}

export default InlineSnippet;
