"use server";

import { signIn } from "@/auth";
import { API_URL } from "@/lib/authHeaders";
import { parseApiError } from "@/lib/apiError";
import type { AuthActionResult } from "@/actions/login";

export async function registerAction(
  _prev: AuthActionResult | undefined,
  formData: FormData,
): Promise<AuthActionResult> {
  const email = String(formData.get("email") ?? "");
  const password = String(formData.get("password") ?? "");
  const displayName = String(formData.get("displayName") ?? "");

  const res = await fetch(`${API_URL}/api/auth/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password, displayName }),
  });

  if (!res.ok) {
    const parsed = parseApiError(await res.json().catch(() => null));
    return { error: parsed?.message ?? "Registration failed.", validationErrors: parsed?.validationErrors };
  }

  // Auto sign-in after a successful registration.
  await signIn("credentials", { email, password, redirect: false });
  return {};
}
