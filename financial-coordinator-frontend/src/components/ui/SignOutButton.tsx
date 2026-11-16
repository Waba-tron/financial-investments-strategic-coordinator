"use client";

import { Button } from "@mui/material";
import { signOut } from "next-auth/react";

export default function SignOutButton() {
  return (
    <Button color="inherit" variant="outlined" onClick={() => signOut({ callbackUrl: "/" })}>
      Sign out
    </Button>
  );
}
