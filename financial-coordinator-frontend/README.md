# financial-coordinator-frontend

Next.js (App Router) + NextAuth v5 (Credentials) + MUI + react-hook-form frontend for the
`FinancialInvestmentStrategicCoordinator` backend.

## Setup

```bash
npm install
# .env.local already holds ECOM_API_URL + AUTH_SECRET; generate a real secret for anything real:
npx auth secret
npm run dev   # http://localhost:3000
```

Make sure the backend is running on `http://localhost:5041` (see `ECOM_API_URL` in `.env.local`).

## How auth works

- `src/auth.ts` — NextAuth Credentials provider. `authorize()` POSTs to `/api/auth/login`; on
  failure it `throw`s the raw backend JSON so the UI can show field-level errors. On success it
  **decodes the JWT payload** into `claims` ({ sub, email, role }) stored on the session.
- `src/middleware.ts` — the template's "proxy". Guards `protectedPaths` (redirects to `/` when
  unauthenticated) and role-gates `/admin` to `role === "Admin"`. *(Next.js requires this file be
  named `middleware.ts`.)*
- `src/lib/authHeaders.ts` — `Authorization: Bearer <token>` for server-side backend calls.
- `src/lib/apiError.ts` — normalizes backend errors; tolerant of **camelCase and PascalCase**.
- `src/actions/*` — Server Actions; forms map returned `validationErrors` (PascalCase → camelCase)
  to react-hook-form `setError`, with unmatched errors shown at form root.

## Routes

| Route | Access |
|---|---|
| `/` | public product catalog |
| `/login`, `/register` | public |
| `/shop` | authenticated |
| `/admin` | `Admin` only (create/delete products) |

## Try it

Log in as `admin@fisco.local` / `Admin1234` → the **Admin** nav link appears and `/admin` lets you
create and delete products. Log in as `customer@fisco.local` / `Password123` → `/admin` redirects
to `/` and the backend returns 403 for write attempts.
