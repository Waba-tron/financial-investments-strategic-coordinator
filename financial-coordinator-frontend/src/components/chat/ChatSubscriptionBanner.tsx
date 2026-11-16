"use client";

import { createCheckoutSession, createPortalSession } from "@/actions/subscription";
import StarIcon from "@mui/icons-material/Star";
import { Alert, Button } from "@mui/material";
import { useState } from "react";

export default function ChatSubscriptionBanner({ plan }: { plan: string }) {
  const isPro = plan === "Pro";
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const go = async (action: typeof createCheckoutSession | typeof createPortalSession) => {
    setError(null);
    setLoading(true);
    const result = await action();
    if (!result.ok || !result.url) {
      setError(result.error ?? "Something went wrong.");
      setLoading(false);
      return;
    }
    // Hand off to Stripe's hosted page.
    window.location.href = result.url;
  };

  return (
    <>
      {isPro ? (
        <Button
          fullWidth
          variant="outlined"
          size="small"
          disabled={loading}
          onClick={() => go(createPortalSession)}
          sx={{ mt: 1.5 }}
        >
          {loading ? "Opening…" : "Manage subscription"}
        </Button>
      ) : (
        <Button
          fullWidth
          variant="contained"
          size="small"
          color="warning"
          startIcon={<StarIcon />}
          disabled={loading}
          onClick={() => go(createCheckoutSession)}
          sx={{ mt: 1.5 }}
        >
          {loading ? "Redirecting…" : "Upgrade to Pro"}
        </Button>
      )}

      {error && (
        <Alert severity="error" sx={{ mt: 1 }}>
          {error}
        </Alert>
      )}
    </>
  );
}
