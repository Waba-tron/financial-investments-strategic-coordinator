"use server";

import { API_URL, authHeaders } from "@/lib/authHeaders";
import { parseApiError } from "@/lib/apiError";

export interface SubscriptionStatus {
  plan: string;
  isSubscribed: boolean;
  subscribedAt: string | null;
}

export interface StripeUrlResult {
  ok: boolean;
  url?: string;
  error?: string;
}

/** Current plan status (GET /api/subscription). */
export async function getSubscription(): Promise<SubscriptionStatus> {
  const response = await fetch(`${API_URL}/api/subscription`, {
    headers: await authHeaders(),
    cache: "no-store",
  });

  if (!response.ok) {
    const parsed = parseApiError(await response.json().catch(() => null));
    throw new Error(parsed?.message ?? "Could not load subscription.");
  }

  return (await response.json()) as SubscriptionStatus;
}

/** Starts a Stripe Checkout session and returns its hosted URL (POST /api/subscription/checkout). */
export async function createCheckoutSession(): Promise<StripeUrlResult> {
  return postForUrl(`${API_URL}/api/subscription/checkout`);
}

/** Opens the Stripe Billing Portal to manage/cancel (POST /api/subscription/portal). */
export async function createPortalSession(): Promise<StripeUrlResult> {
  return postForUrl(`${API_URL}/api/subscription/portal`);
}

async function postForUrl(url: string): Promise<StripeUrlResult> {
  const response = await fetch(url, {
    method: "POST",
    headers: await authHeaders(),
    cache: "no-store",
  });

  if (!response.ok) {
    const parsed = parseApiError(await response.json());
    return { ok: false, error: parsed?.message ?? "Something went wrong. Please try again." };
  }

  const data = (await response.json()) as { url: string };
  return { ok: true, url: data.url };
}
