import { auth } from "@/auth";

export async function authHeaders(){
  const session = await auth();
  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${session?.accessToken}`
  };
}

export const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5041";
