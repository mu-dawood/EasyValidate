import React from 'react';
import ComponentCreator from '@docusaurus/ComponentCreator';

export default [
  {
    path: '/docs',
    component: ComponentCreator('/docs', '45c'),
    routes: [
      {
        path: '/docs',
        component: ComponentCreator('/docs', '231'),
        routes: [
          {
            path: '/docs',
            component: ComponentCreator('/docs', '373'),
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
