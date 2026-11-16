"use server";

import { API_URL, authHeaders } from "@/lib/authHeaders";
import { parseApiError } from "@/lib/apiError";

export interface ConversationSummary {
  id: number;
  title: string;
  updatedDate: string;
}

export interface MessageDto {
  id: number;
  role: "user" | "assistant";
  content: string;
  createdDate: string;
}

export interface ConversationDetail {
  id: number;
  title: string;
  createdDate: string;
  updatedDate: string;
  messages: MessageDto[];
}

export async function getConversations(): Promise<ConversationSummary[]> {
  const response = await fetch(`${API_URL}/api/conversations`, {
    headers: await authHeaders(),
  });

  if (!response.ok) {
    const parsed = parseApiError(await response.json().catch(() => null));
    throw new Error(parsed?.message ?? "Could not load conversations.");
  }

  return (await response.json()) as ConversationSummary[];
}

export async function getConversationById(id: number): Promise<ConversationDetail> {
  const response = await fetch(`${API_URL}/api/conversations/${id}`, {
    headers: await authHeaders(),
  });

  if (!response.ok) {
    const parsed = parseApiError(await response.json());
    throw new Error(parsed?.message ?? "Could not load this conversation.");
  }

  return (await response.json()) as ConversationDetail;
}

export interface DeleteResult {
  ok: boolean;
  error?: string;
}

export async function deleteConversation(id: number): Promise<DeleteResult> {
  const response = await fetch(`${API_URL}/api/conversations/${id}`, {
    method: "DELETE",
    headers: await authHeaders(),
  });

  if (!response.ok) {
    const parsed = parseApiError(await response.json());
    return { ok: false, error: parsed?.message ?? "Could not delete this conversation." };
  }

  return { ok: true };
}
