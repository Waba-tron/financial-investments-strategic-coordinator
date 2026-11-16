# Financial Investments Strategic Coordinator

A financial-assistant web app: a chat bot with usage limits and a Pro subscription
(Stripe). Built as a **.NET Clean Architecture backend** and a **Next.js frontend**.

## Tech stack

| | |
|---|---|
| **Backend** | .NET 10, ASP.NET Core Web API, Clean Architecture (Domain / Application / Infrastructure / API), MediatR (CQRS), FluentValidation, EF Core + SQLite, JWT auth, Stripe.net |
| **Frontend** | Next.js 15 (App Router), React 19, NextAuth v5 (Credentials), MUI, react-hook-form |

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org) 18.18+ (or 20+)
- *(optional, for subscriptions)* a [Stripe](https://dashboard.stripe.com) account + the [Stripe CLI](https://docs.stripe.com/stripe-cli)

## Project layout

```
financial-coordinator-backend/    # .NET solution (API runs on http://localhost:5041)
  …Domain / …Application / …Infrastructure / …API
financial-coordinator-frontend/   # Next.js app (runs on http://localhost:3000)
```

---

## 1. Backend setup

```bash
cd financial-coordinator-backend
dotnet restore
dotnet run --project FinancialInvestmentStrategicCoordinator.API
```

The API starts on **http://localhost:5041** (Swagger UI at `/swagger`).

- The SQLite database is **created automatically on startup**. In Development it is
  **dropped and re-seeded on every run**, so data does not persist between restarts.
- Two users are seeded on startup:

  | Role | Email | Password |
  |---|---|---|
  | Customer | `customer@fisco.local` | `Password123` |

### Stripe config (only needed for subscription features)

`appsettings.json` ships with `CHANGE_ME` placeholders. **Don't put real keys in that
file** — use [user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets):

```bash
cd FinancialInvestmentStrategicCoordinator.API
dotnet user-secrets set "Stripe:SecretKey"     "sk_test_…"   # Dashboard → Developers → API keys
dotnet user-secrets set "Stripe:ProPriceId"    "price_…"     # a recurring price on a product
dotnet user-secrets set "Stripe:WebhookSecret" "whsec_…"     # from `stripe listen` (below)
```

The rest of the app (auth, chat, conversations) runs without any Stripe config.

---

## 2. Frontend setup

Create `financial-coordinator-frontend/.env.local` (it is gitignored):

```bash
NEXT_PUBLIC_API_URL=http://localhost:5041
AUTH_SECRET=<generate one: `npx auth secret`>
```

Then:

```bash
cd financial-coordinator-frontend
npm install
npm run dev
```

Open **http://localhost:3000**. Sign in with a seeded account above, or register a new one.

---

## 3. Testing Stripe subscriptions (optional)

Run the webhook forwarder in its own terminal (keep it running), then set the printed
signing secret as `Stripe:WebhookSecret`:

```bash
stripe listen --all-snapshot --forward-to http://localhost:5041/api/webhooks/stripe
```

Then in the app: **Upgrade to Pro** → pay with Stripe's test card
`4242 4242 4242 4242` (any future expiry, any CVC/ZIP). The `checkout.session.completed`
webhook flips the account to Pro; cancelling via **Manage subscription** downgrades it.

---

## How it works (quick tour)

- **Auth** — register/login issue a JWT; the frontend stores it via NextAuth and sends it
  as a Bearer token. Routes are guarded by middleware.
- **Chat** — `POST /api/chat` returns a reply (currently a stub assistant — swap
  `IChatService` for a real LLM). Free users get a limited number of calls; Pro is unlimited.
- **Conversations** — chats are saved per user; the sidebar lists them.
- **Subscriptions** — Stripe Checkout for upgrades, Billing Portal to manage/cancel, and a
  signature-verified webhook that updates the user's plan.

## Notes

- Secrets (`.env.local`, user-secrets) are never committed — see `.gitignore`.
- The chat assistant is a placeholder (`StubChatService`) until a real LLM provider is wired in.
