"use server";

import { signIn } from "@/auth";
import { extractAuthApiError } from "@/lib/apiError";
import { AuthError } from "next-auth";

export interface AuthActionResult {
  error?: string;
  validationErrors?: Record<string, string[]>;
}

export async function loginAction(
  _prev: AuthActionResult | undefined,
  formData: FormData,
): Promise<AuthActionResult> {
  const email = String(formData.get("email") ?? "");
  const password = String(formData.get("password") ?? "");

  try {
    await signIn("credentials", {
      email,
      password,
      redirect: false,
    });
    return {};
  } catch (error) {
    if (error instanceof AuthError) {
      const parsed = extractAuthApiError(error);
      return {
        error: parsed?.message ?? "Login failed.",
        validationErrors: parsed?.validationErrors,
      };
    }
    throw error;
  }
}
