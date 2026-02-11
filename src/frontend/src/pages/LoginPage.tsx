import { FormEvent, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import api from '../api/client';
import { useAuth } from '../contexts/AuthContext';
import { AuthPayload } from '../types/models';

export const LoginPage = () => {
  const [tenantSlug, setTenantSlug] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const { setSession } = useAuth();
  const navigate = useNavigate();

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      const response = await api.post<AuthPayload>('/api/auth/login', { tenantSlug, email, password });
      setSession(response.data);
      navigate('/dashboard');
    } catch {
      setError('Giriş başarısız. Bilgileri kontrol edin.');
    }
  };

  return (
    <section>
      <h1>mimcrm - Giriş</h1>
      <form onSubmit={submit}>
        <input placeholder="Tenant slug" value={tenantSlug} onChange={(e) => setTenantSlug(e.target.value)} required />
        <input placeholder="E-posta" value={email} onChange={(e) => setEmail(e.target.value)} required />
        <input placeholder="Şifre" type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        <button type="submit">Giriş Yap</button>
      </form>
      {error && <p>{error}</p>}
      <p>
        Hesabın yok mu? <Link to="/register">Kayıt ol</Link>
      </p>
    </section>
  );
};
