"use client";

export default function GlobalError({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {

  return (
    <html lang="en">
      <body style={{ fontFamily: "system-ui, sans-serif", margin: 0 }}>
        <div style={{ maxWidth: 560, margin: "0 auto", padding: "48px 16px" }}>
          <h1 style={{ marginBottom: 8 }}>Something went wrong</h1>
          <p style={{ color: "#b00020" }}>
            {error.message || "An unexpected error occurred."}
          </p>
          {error.digest && (
            <p style={{ fontSize: 12, opacity: 0.7 }}>Reference: {error.digest}</p>
          )}
          <button
            onClick={reset}
            style={{
              marginTop: 16,
              padding: "8px 16px",
              border: "none",
              borderRadius: 4,
              background: "#1b4965",
              color: "#fff",
              cursor: "pointer",
            }}
          >
            Try again
          </button>
        </div>
      </body>
    </html>
  );
}
