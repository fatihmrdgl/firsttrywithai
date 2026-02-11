import { Navigate } from 'react-router-dom';
import { PropsWithChildren } from 'react';
import { useAuth } from '../contexts/AuthContext';

export const ProtectedRoute = ({ children }: PropsWithChildren) => {
  const { auth } = useAuth();

  if (!auth?.token) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
};
