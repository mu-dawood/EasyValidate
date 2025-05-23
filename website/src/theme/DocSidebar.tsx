import React from 'react';
import type { Props } from '@theme/DocSidebar';
import OriginalDocSidebar from '@theme-original/DocSidebar';
import SidebarLogo from '../components/SidebarLogo';

export default function DocSidebar(props: Props) {
  return (
    <>
      <SidebarLogo />
      <OriginalDocSidebar {...props} />
    </>
  );
}
