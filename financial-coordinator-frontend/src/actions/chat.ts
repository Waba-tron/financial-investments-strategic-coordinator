"use server";

import { API_URL, authHeaders } from "@/lib/authHeaders";
import { parseApiError } from "@/lib/apiError";

export interface ChatResponse {
  reply: string;
  callsUsed: number;
  callsRemaining: number | null;
  conversationId: number;
  title: string;
}

export interface UsageResponse {
  plan: string;
  callsUsed: number;
  callsRemaining: number | null;
}

export interface SendMessageResult {
  ok: boolean;
  error?: string;
  errorCode?: string;
  validationErrors?: Record<string, string[]>;
  data?: ChatResponse;
}

export async function sendChatMessage(
  message: string,
  conversationId?: number,
): Promise<SendMessageResult> {


  const response = await fetch(`${API_URL}/api/chat`, {
    method: "POST",
    headers: await authHeaders(),
    body: JSON.stringify({ message, conversationId }),
  });



  if (!response.ok) {

    const parsed = parseApiError(await response.json());

    return {
      ok: false,
      error: parsed?.message ?? "Error when sending chat.",
      errorCode: parsed?.errorCode,
      validationErrors: parsed?.validationErrors,
    };
  }

  return { ok: true, data: (await response.json()) as ChatResponse };
}

export async function getUsage(): Promise<UsageResponse> {
  const response = await fetch(`${API_URL}/api/chat/usage`, {
    headers: await authHeaders(),
  });
  

  if (!response.ok) {
    const parsed = parseApiError(await response.json());
    throw new Error(parsed?.message ?? "Error when getting usage.");
  }

  return (await response.json()) as UsageResponse;
}
