"use client";

import LoginForm from "@/components/auth/LoginForm";
import RegisterForm from "@/components/auth/RegisterForm";
import { Box, Button, Paper, Stack } from "@mui/material";
import { useState } from "react";

type Mode = "login" | "register";

export default function AuthPage() {
  const [mode, setMode] = useState<Mode>("login");

  return (
    <Box
      sx={{
        minHeight: "70vh",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
      }}
    >
      <Paper
        elevation={3}
        sx={{
          width: "100%",
          maxWidth: 440,
          p: { xs: 3, sm: 5 },
          borderRadius: 3,
        }}
      >
        <Stack direction="row" spacing={2} sx={{ mb: 4 }}>
          <Button
            fullWidth
            variant={mode === "login" ? "contained" : "outlined"}
            onClick={() => setMode("login")}
          >
            Login
          </Button>
          <Button
            fullWidth
            variant={mode === "register" ? "contained" : "outlined"}
            onClick={() => setMode("register")}
          >
            Register
          </Button>
        </Stack>

        {mode === "login" ? <LoginForm /> : <RegisterForm />}
      </Paper>
    </Box>
  );
}
