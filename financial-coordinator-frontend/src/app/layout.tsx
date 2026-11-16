import type { Metadata } from "next";
import Providers from "./providers";
import NavBar from "@/components/ui/NavBar";

export const metadata: Metadata = {
  title: "Financial Investment Strategic Co-coordinator",
  description: "Clean Architecture + Auth/Roles starter frontend",
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <Providers>
          <NavBar />
          <main style={{ maxWidth: 960, margin: "0 auto", padding: "24px 16px" }}>
            {children}
          </main>
        </Providers>
      </body>
    </html>
  );
}
