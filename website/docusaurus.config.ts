import type { Config } from '@docusaurus/types';
import { themes } from 'prism-react-renderer';
import sidebarPath from './sidebars';
import customCss from './src/css/custom.css';

const config: Config = {
  title: 'EasyValidate',
  tagline: 'Modern, fast, and extensible attribute-based validation for .NET',
  url: 'https://yourdomain.com',
  baseUrl: '/',
  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',
  favicon: 'img/favicon.ico',
  organizationName: 'your-org',
  projectName: 'EasyValidate',
  themeConfig: {
    // Dyte-style: no top navbar, but a prominent sidebar with logo at the top
    navbar: undefined,
    docs: {
      sidebar: {
        hideable: true,
        autoCollapseCategories: true,
        // We'll style the sidebar to include the logo at the top
      },
    },
    footer: undefined,
    colorMode: {
      defaultMode: 'light',
      disableSwitch: true,
      respectPrefersColorScheme: false,
    },
    prism: {
      theme: themes.github,
    },
  },
  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: sidebarPath,
          editUrl: 'https://github.com/your-org/EasyValidate/edit/main/website/',
        },
        
        theme: {
          customCss: customCss,
        },
      },
    ],
  ],
};

export default config;
