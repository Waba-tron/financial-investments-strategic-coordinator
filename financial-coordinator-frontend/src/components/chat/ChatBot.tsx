"use client";

import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Paper,
  TextField,
  Typography,
} from "@mui/material";
import { useEffect, useRef, useState } from "react";

export interface ChatMessage {
  role: "user" | "assistant";
  content: string;
}

interface ChatBotProps {
  messages: ChatMessage[];
  sending: boolean;
  loading: boolean;
  error: string | null;
  quotaReached: boolean;
  onSend: (message: string) => void;
}

export default function ChatBot({ messages, sending, loading, error, quotaReached, onSend }: ChatBotProps) {
  const [message, setMessage] = useState("");
  const endRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    endRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages, sending]);

  const submit = () => {
    const trimmed = message.trim();
    if (!trimmed || sending) return;
    onSend(trimmed);
    setMessage("");
  };

  const onSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    submit();
  };

  return (
    <Box sx={{ flex: 1, minWidth: 0, display: "flex", flexDirection: "column", height: "100%", p: 2 }}>
      <Typography variant="h5" sx={{ mb: 2 }}>
        Financial Assistant
      </Typography>

      <Paper
        variant="outlined"
        sx={{
          flex: 1,
          overflowY: "auto",
          p: 2,
          mb: 2,
          display: "flex",
          flexDirection: "column",
          gap: 1.5,
          bgcolor: "grey.50",
        }}
      >
        {loading ? (
          <Box sx={{ m: "auto" }}>
            <CircularProgress size={24} />
          </Box>
        ) : messages.length === 0 ? (
          <Typography color="text.secondary" sx={{ m: "auto", textAlign: "center" }}>
            Ask me anything about your finances or investments to get started.
          </Typography>
        ) : (
          messages.map((m, i) => (
            <Box
              key={i}
              sx={{
                alignSelf: m.role === "user" ? "flex-end" : "flex-start",
                maxWidth: "80%",
              }}
            >
              <Paper
                elevation={0}
                sx={{
                  px: 2,
                  py: 1,
                  borderRadius: 2,
                  bgcolor: m.role === "user" ? "primary.main" : "background.paper",
                  color: m.role === "user" ? "primary.contrastText" : "text.primary",
                  border: m.role === "user" ? "none" : "1px solid",
                  borderColor: "divider",
                  whiteSpace: "pre-wrap",
                }}
              >
                <Typography variant="body1">{m.content}</Typography>
              </Paper>
            </Box>
          ))
        )}

        {sending && (
          <Box sx={{ alignSelf: "flex-start", display: "flex", alignItems: "center", gap: 1, px: 1 }}>
            <CircularProgress size={16} />
            <Typography variant="body2" color="text.secondary">
              Thinking…
            </Typography>
          </Box>
        )}

        <div ref={endRef} />
      </Paper>

      {error && (
        <Alert severity={quotaReached ? "warning" : "error"} sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}

      <Box component="form" onSubmit={onSubmit} sx={{ display: "flex", gap: 1 }}>
        <TextField
          fullWidth
          placeholder={quotaReached ? "Free limit reached — subscribe to continue" : "Type your message…"}
          value={message}
          onChange={(e) => setMessage(e.target.value)}
          disabled={sending || quotaReached}
          multiline
          maxRows={4}
          onKeyDown={(e) => {
            if (e.key === "Enter" && !e.shiftKey) {
              e.preventDefault();
              submit();
            }
          }}
        />
        <Button
          type="submit"
          variant="contained"
          disabled={sending || quotaReached || !message.trim()}
          sx={{ px: 3 }}
        >
          Send
        </Button>
      </Box>
    </Box>
  );
}
