import NextAuth from "next-auth";
import Credentials from "next-auth/providers/credentials";

const API_URL = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5041";

export interface JwtClaims {
  sub?: string;
  email?: string;
  role?: string;
  [key: string]: unknown;
}

/** Decodes a JWT payload WITHOUT verifying the signature (backend already signed it). */
function decodeJwt(token: string): JwtClaims {
  return JSON.parse(Buffer.from(token.split(".")[1], "base64url").toString("utf8"))
}

export const { handlers, auth, signIn, signOut } = NextAuth({
  providers: [
    Credentials({
      credentials: {
        email: { label: "Email", type: "email" },
        password: { label: "Password", type: "password" },
      },
      async authorize(credentials) {
        const res = await fetch(`${API_URL}/api/auth/login`, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({
            email: credentials?.email,
            password: credentials?.password,
          }),
        });

        const body = await res.text();

        if (!res.ok) {
          throw new Error(body);
        }

        const data = JSON.parse(body) as { token: string; displayName: string };
        const claims = decodeJwt(data.token);

        return {
          id: claims.sub ?? "",
          name: data.displayName,
          email: claims.email ?? (credentials?.email as string) ?? "",
          accessToken: data.token,
          claims,
        };
      },
    }),
  ],
  pages: {
    signIn: "/login",
  },
  callbacks: {
    async jwt({ token, user }) {
      if (user) {
        token.accessToken = user.accessToken;
        token.claims = user.claims;
      }
      return token;
    },
    async session({ session, token }) {
      session.accessToken = token.accessToken as string | undefined;
      session.claims = token.claims as JwtClaims | undefined;
      return session;
    },
  },
});
