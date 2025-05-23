import React from 'react';
import DocSidebar from '@theme-original/DocSidebar';
export default function DocSidebarWrapper(props) {
    return (React.createElement(React.Fragment, null,
        React.createElement(DocSidebar, { ...props })));
}
