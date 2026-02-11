import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import { AuthProvider } from './contexts/AuthContext';
import { setAuthHeaders } from './api/client';

const storedAuth = localStorage.getItem('mimcrm.auth');
if (storedAuth) {
  const session = JSON.parse(storedAuth) as { token: string; tenantId: string };
  setAuthHeaders(session.token, session.tenantId);
}

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <App />
      </AuthProvider>
    </BrowserRouter>
  </React.StrictMode>
);
