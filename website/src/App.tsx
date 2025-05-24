import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import Header from './components/Header';
import Footer from './components/Footer';
import ScrollToTop from './components/ScrollToTop';
import DocumentationLayout from './components/DocumentationLayout';
import Home from './pages/Home';
import DocsOverview from './pages/DocsOverview';
import RealWorldExamples from './pages/RealWorldExamples';
import GettingStarted from './pages/GettingStarted';
import ValidationAttributes from './pages/ValidationAttributes';
import AdvancedUsage from './pages/AdvancedUsage';
import ApiReference from './pages/ApiReference';
import './App.css';

function AppContent() {
  const location = useLocation();
  const isDocsPage = location.pathname.startsWith('/docs');

  return (
    <div className="app">
      {!isDocsPage && <Header />}
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/docs" element={<DocumentationLayout />}>
          <Route index element={<DocsOverview />} />
          <Route path="getting-started" element={<GettingStarted />} />
          <Route path="examples" element={<RealWorldExamples />} />
          <Route path="attributes" element={<ValidationAttributes />} />
          <Route path="advanced" element={<AdvancedUsage />} />
          <Route path="api" element={<ApiReference />} />
        </Route>
      </Routes>
      {!isDocsPage && <Footer />}
      <ScrollToTop />
    </div>
  );
}

function App() {
  return (
    <Router>
      <AppContent />
    </Router>
  );
}

export default App;
