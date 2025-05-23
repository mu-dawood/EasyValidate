import type { SidebarsConfig } from '@docusaurus/plugin-content-docs';

const sidebars: SidebarsConfig = {
  docs: [
    {
      type: 'category',
      label: 'Getting Started',
      items: ['intro', 'installation', 'quickstart'],
    },
    {
      type: 'category',
      label: 'Attributes',
      items: ['attributes'],
    },
    'extending',
    'analyzers',
    'localization',
    'contributing',
  ],
};

export default sidebars;
