import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../api/client';
import { useAuth } from '../contexts/AuthContext';
import { AuthPayload } from '../types/models';

export const RegisterPage = () => {
  const [tenantName, setTenantName] = useState('');
  const [tenantSlug, setTenantSlug] = useState('');
  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const { setSession } = useAuth();
  const navigate = useNavigate();

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      const response = await api.post<AuthPayload>('/api/auth/register', {
        tenantName,
        tenantSlug,
        fullName,
        email,
        password
      });
      setSession(response.data);
      navigate('/dashboard');
    } catch {
      setError('Kayıt başarısız. Tenant slug farklı olmalı.');
    }
  };

  return (
    <section>
      <h1>mimcrm - Kayıt</h1>
      <form onSubmit={submit}>
        <input placeholder="Firma adı" value={tenantName} onChange={(e) => setTenantName(e.target.value)} required />
        <input placeholder="Tenant slug" value={tenantSlug} onChange={(e) => setTenantSlug(e.target.value)} required />
        <input placeholder="Ad Soyad" value={fullName} onChange={(e) => setFullName(e.target.value)} required />
        <input placeholder="E-posta" value={email} onChange={(e) => setEmail(e.target.value)} required />
        <input placeholder="Şifre" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        <button type="submit">Kayıt Ol</button>
      </form>
      {error && <p>{error}</p>}
      <p>
        Zaten hesabın var mı? <Link to="/login">Giriş yap</Link>
      </p>
    </section>
  );
};
