import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:8080',
});

export const setAuthHeaders = (token?: string, tenantId?: string) => {
  if (token) {
    api.defaults.headers.common.Authorization = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common.Authorization;
  }

  if (tenantId) {
    api.defaults.headers.common['X-Tenant-Id'] = tenantId;
  } else {
    delete api.defaults.headers.common['X-Tenant-Id'];
  }
};

export default api;
