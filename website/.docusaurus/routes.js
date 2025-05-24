import React from 'react';
import ComponentCreator from '@docusaurus/ComponentCreator';

export default [
  {
    path: '/__docusaurus/debug',
    component: ComponentCreator('/__docusaurus/debug', '5ff'),
    exact: true
  },
  {
    path: '/__docusaurus/debug/config',
    component: ComponentCreator('/__docusaurus/debug/config', '5ba'),
    exact: true
  },
  {
    path: '/__docusaurus/debug/content',
    component: ComponentCreator('/__docusaurus/debug/content', 'a2b'),
    exact: true
  },
  {
    path: '/__docusaurus/debug/globalData',
    component: ComponentCreator('/__docusaurus/debug/globalData', 'c3c'),
    exact: true
  },
  {
    path: '/__docusaurus/debug/metadata',
    component: ComponentCreator('/__docusaurus/debug/metadata', '156'),
    exact: true
  },
  {
    path: '/__docusaurus/debug/registry',
    component: ComponentCreator('/__docusaurus/debug/registry', '88c'),
    exact: true
  },
  {
    path: '/__docusaurus/debug/routes',
    component: ComponentCreator('/__docusaurus/debug/routes', '000'),
    exact: true
  },
  {
    path: '/docs',
    component: ComponentCreator('/docs', '9a7'),
    routes: [
      {
        path: '/docs',
        component: ComponentCreator('/docs', '52d'),
        routes: [
          {
            path: '/docs',
            component: ComponentCreator('/docs', '78c'),
            routes: [
              {
                path: '/docs/analyzers',
                component: ComponentCreator('/docs/analyzers', 'bf7'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/attributes',
                component: ComponentCreator('/docs/attributes', '3ba'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/contributing',
                component: ComponentCreator('/docs/contributing', '476'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/example-usage',
                component: ComponentCreator('/docs/example-usage', 'f21'),
                exact: true
              },
              {
                path: '/docs/extending',
                component: ComponentCreator('/docs/extending', '0bf'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/installation',
                component: ComponentCreator('/docs/installation', '057'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/intro',
                component: ComponentCreator('/docs/intro', 'a6e'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/localization',
                component: ComponentCreator('/docs/localization', 'fae'),
                exact: true,
                sidebar: "docs"
              },
              {
                path: '/docs/multi-window-example',
                component: ComponentCreator('/docs/multi-window-example', '9cd'),
                exact: true
              },
              {
                path: '/docs/quickstart',
                component: ComponentCreator('/docs/quickstart', '5e3'),
                exact: true,
                sidebar: "docs"
              }
            ]
          }
        ]
      }
    ]
  },
  {
    path: '/',
    component: ComponentCreator('/', '2e1'),
    exact: true
  },
  {
    path: '*',
    component: ComponentCreator('*'),
  },
];
