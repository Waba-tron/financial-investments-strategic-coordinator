"use client";

// App-wide error boundary. Catches errors thrown while rendering any route segment
// that doesn't have its own error.tsx — including data-fetch throws in server pages
// like the chat page's getUsage()/getConversations().
import { Alert, AlertTitle, Box, Button, Stack, Typography } from "@mui/material";
import Link from "next/link";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {

  return (
    <Box sx={{ maxWidth: 560, mx: "auto", py: 6 }}>
      <Typography variant="h4" gutterBottom>
        Something went wrong
      </Typography>

      <Alert severity="error" sx={{ mb: 3 }}>
        <AlertTitle>Error</AlertTitle>
        {error.message || "An unexpected error occurred."}
        {error.digest && (
          <Typography variant="caption" component="div" sx={{ mt: 1, opacity: 0.7 }}>
            Reference: {error.digest}
          </Typography>
        )}
      </Alert>

      <Stack direction="row" spacing={2}>
        <Button variant="contained" onClick={reset}>
          Try again
        </Button>
        <Button variant="outlined" component={Link} href="/">
          Go home
        </Button>
      </Stack>
    </Box>
  );
}
