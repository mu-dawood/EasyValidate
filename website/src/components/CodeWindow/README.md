# CodeWindow Footer Feature

The `CodeWindow` component now supports an optional footer that can display additional information, actions, or links below the code content.

## Usage

```tsx
import CodeWindow from './components/CodeWindow';

// Basic usage with footer
<CodeWindow 
  windows={codeWindows}
  footer={
    <div>
      <span>💡 Helpful tip or information</span>
      <button>Action Button</button>
    </div>
  }
/>
```

## Props

The `footer` prop accepts any `React.ReactNode`, giving you complete flexibility in what you display:

```tsx
interface MultiWindowProps {
  windows: Window[];
  className?: string;
  showCopyButton?: boolean;
  variant?: 'default' | 'hero' | 'colorful' | 'light';
  footer?: React.ReactNode; // ✨ New optional prop
}
```

## Examples

### Simple Text Footer
```tsx
<CodeWindow 
  windows={codeWindows}
  footer="💡 This example shows basic validation usage"
/>
```

### Footer with Actions
```tsx
<CodeWindow 
  windows={codeWindows}
  footer={
    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
      <span>Try this example in your project</span>
      <div>
        <a href="#docs">Learn More</a>
        <button onClick={handleCopy}>Copy Template</button>
      </div>
    </div>
  }
/>
```

### Footer with Custom Styling
```tsx
<CodeWindow 
  windows={codeWindows}
  footer={
    <div className="custom-footer">
      <div className="footer-text">
        <span>🚀 Performance tip: Use compile-time validation</span>
      </div>
      <div className="footer-actions">
        <button className="primary-btn">Try Now</button>
        <button className="secondary-btn">Learn More</button>
      </div>
    </div>
  }
/>
```

## Styling

The footer is automatically styled with:
- Background color that matches the CodeWindow theme
- Proper spacing and padding
- Responsive design for mobile devices
- Consistent typography and colors

You can override the default styles by targeting the `.codeFooter` CSS class or by using inline styles in your footer content.

## CSS Classes Available

The footer container uses the `.codeFooter` class with these predefined styles:
- Background and border styling
- Flexbox layout for easy content arrangement
- Responsive behavior
- Consistent spacing

## Best Practices

1. **Keep it concise**: Footers should provide helpful but brief information
2. **Use semantic elements**: Use proper buttons, links, and text elements
3. **Consider responsive design**: Test footer content on mobile devices
4. **Maintain accessibility**: Ensure footer content is accessible to screen readers
5. **Match the theme**: Use colors and styling that complement the CodeWindow variant

## Implementation Details

The footer feature is implemented as:
- An optional prop that accepts any React node
- Conditionally rendered below the code content
- Styled with a separate CSS class for customization
- Responsive design that adapts to screen size

This feature enhances the CodeWindow component by providing space for:
- Explanatory text or tips
- Action buttons (copy, try, learn more)
- Links to documentation or examples
- Interactive elements related to the code sample
