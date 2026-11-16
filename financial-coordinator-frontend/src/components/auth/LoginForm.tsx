"use client";

import { loginAction } from "@/actions/login";
import { toCamelCase } from "@/lib/apiError";
import { Alert, Box, Button, TextField, Typography } from "@mui/material";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { Controller, useForm } from "react-hook-form";

interface FormValues {
  email: string;
  password: string;
}

export default function LoginForm() {
  const router = useRouter();
  const [rootError, setRootError] = useState<string | null>(null);
  const { control, handleSubmit, setError, formState } = useForm<FormValues>({
    defaultValues: { email: "", password: "" },
  });

  const onSubmit = async (values: FormValues) => {
    setRootError(null);
    const formData = new FormData();
    formData.set("email", values.email);
    formData.set("password", values.password);

    const result = await loginAction(undefined, formData);

    if (!result.error && !result.validationErrors) {
      router.push("/");
      router.refresh();
      return;
    }

    const fieldErrors = result.validationErrors ?? {};
    let matched = false;
    
    for (const [key, messages] of Object.entries(fieldErrors)) {
      const field = toCamelCase(key) as keyof FormValues;
      if (field === "email" || field === "password") {
        setError(field, { message: messages.join(" ") });
        matched = true;
      }
    }
    if (!matched) {
      setRootError(result.error ?? "Login failed.");
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit(onSubmit)} sx={{ display: "grid", gap: 2, maxWidth: 400 }}>
      <Typography variant="h4">Login</Typography>
      {rootError && <Alert severity="error">{rootError}</Alert>}

      <Controller
        name="email"
        control={control}
        render={({ field, fieldState }) => (
          <TextField {...field} label="Email" type="email" error={!!fieldState.error} helperText={fieldState.error?.message} />
        )}
      />
      <Controller
        name="password"
        control={control}
        render={({ field, fieldState }) => (
          <TextField {...field} label="Password" type="password" error={!!fieldState.error} helperText={fieldState.error?.message} />
        )}
      />

      <Button type="submit" variant="contained" disabled={formState.isSubmitting}>
        {formState.isSubmitting ? "Signing in…" : "Sign in"}
      </Button>
    </Box>
  );
}
