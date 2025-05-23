import React from 'react';
import OriginalDocSidebar from '@theme-original/DocSidebar';
import SidebarLogo from '../components/SidebarLogo';
export default function DocSidebar(props) {
    return (React.createElement(React.Fragment, null,
        React.createElement(SidebarLogo, null),
        React.createElement(OriginalDocSidebar, { ...props })));
}
