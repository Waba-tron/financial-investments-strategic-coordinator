// Route middleware (the template refers to this as "proxy.ts"). Next.js requires the
// file be named `middleware.ts`. It guards authenticated routes.
import { auth } from "@/auth";
import { NextResponse } from "next/server";

// Authenticated-only prefixes.
const protectedPaths = ["/chat"];

export default auth((req) => {
  const { pathname } = req.nextUrl;
  const session = req.auth;

  const isProtected = protectedPaths.some(
    (p) => pathname === p || pathname.startsWith(`${p}/`),
  );

  // Home page ("/") is the login/register screen. If you're already signed in,
  // skip it and go straight to the chat.
  if (pathname === "/" && session) {
    return NextResponse.redirect(new URL("/chat", req.nextUrl));
  }

  if (isProtected && !session) {
    return NextResponse.redirect(new URL("/", req.nextUrl));
  }

  return NextResponse.next();
});