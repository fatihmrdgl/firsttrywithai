# mimcrm

.NET Core + React tabanlı, multi-tenant, abonelik modeline uygun kurumsal CRM başlangıç projesi.

## Özellikler

- **Multi-tenant mimari** (`X-Tenant-Id` header ile tenant çözümleme)
- **Auth servisi** (register/login, JWT üretimi, tenant bazlı kullanıcı izolasyonu)
- **MySQL + Entity Framework Core**
- **REST API** (Auth, Product, Customer)
- **GraphQL API** (`/graphql` query + mutation)
- **Loglama** (Serilog)
- **Zamanlanmış görevler** (Hangfire ile abonelik süresi kontrolü)
- **Frontend** (React + Vite, kayıt, login/logout, ürün/müşteri yönetimi)
- **Container desteği** (Dockerfile + docker-compose)
- **Kubernetes manifestleri** (api, web, mysql)

## Proje yapısı

```text
src/
  backend/
    MimCrm.Api/        # ASP.NET Core API (REST + GraphQL)
  frontend/            # React UI
k8s/                   # Kubernetes deployment dosyaları
```

## Backend uç noktaları

### REST

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/products` (Auth + Tenant)
- `POST /api/products` (Admin + Auth + Tenant)
- `GET /api/customers` (Auth + Tenant)
- `POST /api/customers` (Admin + Auth + Tenant)

### GraphQL

- Endpoint: `POST /graphql`
- Playground: `GET /graphql`

Örnek query:

```graphql
query {
  products {
    id
    name
    price
  }
  customers {
    id
    name
    email
  }
}
```

Örnek mutation:

```graphql
mutation {
  createProduct(request: { name: "CRM Paketi", description: "Pro", price: 99.9 }) {
    id
    name
  }
}
```

> GraphQL ve REST çağrılarında `Authorization: Bearer <token>` ve `X-Tenant-Id: <tenantId|tenantSlug>` header'larını gönderin.

## Çalıştırma (Docker Compose)

```bash
docker compose up --build
```

- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`
- Hangfire Dashboard: `http://localhost:8080/jobs`
- GraphQL: `http://localhost:8080/graphql`
- Web: `http://localhost:5173`

## Lokal geliştirme

### Frontend

```bash
cd src/frontend
npm install
npm run dev
```

### Backend

```bash
cd src/backend/MimCrm.Api
dotnet restore
dotnet run
```

## Kubernetes

```bash
kubectl apply -f k8s/mysql.yaml
kubectl apply -f k8s/api.yaml
kubectl apply -f k8s/web.yaml
```

## Ürünleştirme notları

- JWT secret ve DB şifrelerini production ortamında secret manager ile yönetin.
- Tenant onboarding, abonelik faturalama (Stripe/Iyzico) ve email doğrulama akışları ekleyin.
- EF migrations ve CI/CD pipeline (lint/test/build/deploy) eklenmesi önerilir.
