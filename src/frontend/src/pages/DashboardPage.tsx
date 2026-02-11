import { FormEvent, useEffect, useState } from 'react';
import api from '../api/client';
import { useAuth } from '../contexts/AuthContext';
import { Customer, Product } from '../types/models';

export const DashboardPage = () => {
  const { auth, logout } = useAuth();
  const [products, setProducts] = useState<Product[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [productName, setProductName] = useState('');
  const [productDescription, setProductDescription] = useState('');
  const [productPrice, setProductPrice] = useState('0');
  const [customerName, setCustomerName] = useState('');
  const [customerEmail, setCustomerEmail] = useState('');
  const [customerPhone, setCustomerPhone] = useState('');

  const load = async () => {
    const [p, c] = await Promise.all([
      api.get<Product[]>('/api/products'),
      api.get<Customer[]>('/api/customers')
    ]);

    setProducts(p.data);
    setCustomers(c.data);
  };

  useEffect(() => {
    load();
  }, []);

  const createProduct = async (e: FormEvent) => {
    e.preventDefault();
    await api.post('/api/products', {
      name: productName,
      description: productDescription,
      price: Number(productPrice)
    });
    setProductName('');
    setProductDescription('');
    setProductPrice('0');
    await load();
  };

  const createCustomer = async (e: FormEvent) => {
    e.preventDefault();
    await api.post('/api/customers', {
      name: customerName,
      email: customerEmail,
      phone: customerPhone
    });
    setCustomerName('');
    setCustomerEmail('');
    setCustomerPhone('');
    await load();
  };

  return (
    <section>
      <h1>mimcrm Dashboard</h1>
      <p>Tenant: {auth?.tenantId}</p>
      <button onClick={logout}>Logout</button>

      <h2>Yeni Ürün (Admin)</h2>
      <form onSubmit={createProduct}>
        <input placeholder="Ürün adı" value={productName} onChange={(e) => setProductName(e.target.value)} required />
        <input placeholder="Açıklama" value={productDescription} onChange={(e) => setProductDescription(e.target.value)} required />
        <input placeholder="Fiyat" type="number" value={productPrice} onChange={(e) => setProductPrice(e.target.value)} required />
        <button type="submit">Ürün Ekle</button>
      </form>

      <h2>Yeni Müşteri (Admin)</h2>
      <form onSubmit={createCustomer}>
        <input placeholder="Müşteri adı" value={customerName} onChange={(e) => setCustomerName(e.target.value)} required />
        <input placeholder="E-posta" value={customerEmail} onChange={(e) => setCustomerEmail(e.target.value)} required />
        <input placeholder="Telefon" value={customerPhone} onChange={(e) => setCustomerPhone(e.target.value)} required />
        <button type="submit">Müşteri Ekle</button>
      </form>

      <h2>Ürünler</h2>
      <ul>
        {products.map((product) => (
          <li key={product.id}>
            {product.name} - {product.price}
          </li>
        ))}
      </ul>

      <h2>Müşteriler</h2>
      <ul>
        {customers.map((customer) => (
          <li key={customer.id}>
            {customer.name} - {customer.email}
          </li>
        ))}
      </ul>
    </section>
  );
};
