#!/usr/bin/env python3

import re

def fix_inline_snippets(content):
    # Pattern to match InlineSnippet blocks
    pattern = r'<InlineSnippet language="csharp">\s*(.*?)\s*</InlineSnippet>'
    
    def replacement(match):
        code_content = match.group(1).strip()
        # Remove any existing template literal markers
        code_content = re.sub(r'^\{\`\s*', '', code_content)
        code_content = re.sub(r'\s*\`\}$', '', code_content)
        
        return f'<InlineSnippet language="csharp">\n{{\`\n{code_content}\n\`}}\n</InlineSnippet>'
    
    # Replace all InlineSnippet blocks
    result = re.sub(pattern, replacement, content, flags=re.DOTALL)
    return result

# Read the file
with open('/Users/mu.dawood/Desktop/Apps/EasyValidate/website/docs/quickstart.md', 'r') as f:
    content = f.read()

# Fix the snippets
fixed_content = fix_inline_snippets(content)

# Write back
with open('/Users/mu.dawood/Desktop/Apps/EasyValidate/website/docs/quickstart.md', 'w') as f:
    f.write(fixed_content)

print("Fixed all InlineSnippet blocks in quickstart.md")
