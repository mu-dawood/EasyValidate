import type { Config } from '@docusaurus/types';
import { themes } from 'prism-react-renderer';

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
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Docs',
          items: [
            { label: 'Getting Started', to: '/docs/intro' },
            { label: 'Attributes', to: '/docs/attributes' },
            { label: 'Extending', to: '/docs/extending' },
          ],
        },
        {
          title: 'Community',
          items: [
            { label: 'GitHub', href: 'https://github.com/your-org/EasyValidate' },
            { label: 'NuGet', href: 'https://www.nuget.org/packages/EasyValidate' },
          ],
        },
      ],
      copyright: `© ${new Date().getFullYear()} EasyValidate Authors. MIT License.`,
    },
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
          sidebarPath: require.resolve('./sidebars.ts'),
          editUrl: 'https://github.com/your-org/EasyValidate/edit/main/website/',
        },
        theme: {
          customCss: require.resolve('./src/css/custom.css'),
        },
      },
    ],
  ],
};

export default config;
