"use client";

import { Alert, Collapse } from "@mui/material";
import { useState } from "react";

// Shown after returning from Stripe Checkout (success/cancel url -> /chat?subscribed=...).
export default function CheckoutBanner({ status }: { status: "success" | "canceled" }) {
  const [open, setOpen] = useState(true);

  return (
    <Collapse in={open}>
      {status === "success" ? (
        <Alert severity="success" onClose={() => setOpen(false)} sx={{ mb: 2 }}>
          Thanks for subscribing! Your Pro plan is activated
        </Alert>
      ) : (
        <Alert severity="info" onClose={() => setOpen(false)} sx={{ mb: 2 }}>
          Checkout canceled — you’re still on the Free plan.
        </Alert>
      )}
    </Collapse>
  );
}
