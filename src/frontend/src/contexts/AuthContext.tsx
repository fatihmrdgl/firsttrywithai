import { createContext, PropsWithChildren, useContext, useMemo, useState } from 'react';
import { setAuthHeaders } from '../api/client';
import { AuthPayload } from '../types/models';

type AuthContextValue = {
  auth?: AuthPayload;
  setSession: (auth?: AuthPayload) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export const AuthProvider = ({ children }: PropsWithChildren) => {
  const [auth, setAuth] = useState<AuthPayload | undefined>(() => {
    const raw = localStorage.getItem('mimcrm.auth');
    return raw ? (JSON.parse(raw) as AuthPayload) : undefined;
  });

  const setSession = (next?: AuthPayload) => {
    setAuth(next);
    if (next) {
      localStorage.setItem('mimcrm.auth', JSON.stringify(next));
      setAuthHeaders(next.token, next.tenantId);
    } else {
      localStorage.removeItem('mimcrm.auth');
      setAuthHeaders();
    }
  };

  const logout = () => setSession(undefined);

  const value = useMemo(() => ({ auth, setSession, logout }), [auth]);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return ctx;
};
