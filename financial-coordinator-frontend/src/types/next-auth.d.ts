import type { JwtClaims } from "@/auth";
import type { DefaultSession } from "next-auth";

declare module "next-auth" {
  interface User {
    accessToken?: string;
    claims?: JwtClaims;
  }

  interface Session {
    accessToken?: string;
    claims?: JwtClaims;
    user: DefaultSession["user"];
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken?: string;
    claims?: JwtClaims;
  }
}
