export type AuthPayload = {
  token: string;
  userId: string;
  tenantId: string;
  role: string;
};

export type Product = {
  id: string;
  name: string;
  description: string;
  price: number;
};

export type Customer = {
  id: string;
  name: string;
  email: string;
  phone: string;
};
