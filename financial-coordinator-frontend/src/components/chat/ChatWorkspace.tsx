"use client";

import { sendChatMessage, type UsageResponse } from "@/actions/chat";
import {
  deleteConversation,
  getConversation,
  type ConversationSummary,
} from "@/actions/conversations";
import ChatBot, { type ChatMessage } from "@/components/chat/ChatBot";
import ChatSidebar from "@/components/chat/ChatSidebar";
import { Box } from "@mui/material";
import { useState } from "react";

interface ChatWorkspaceProps {
  initialUsage: UsageResponse;
  initialConversations: ConversationSummary[];
}

export default function ChatWorkspace({ initialUsage, initialConversations }: ChatWorkspaceProps) {
  const [usage, setUsage] = useState<UsageResponse>(initialUsage);
  const [conversations, setConversations] = useState<ConversationSummary[]>(initialConversations);
  const [activeId, setActiveId] = useState<number | null>(null);
  const [messages, setMessages] = useState<ChatMessage[]>([]);

  const [sending, setSending] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [quotaReached, setQuotaReached] = useState(false);

  const startNewChat = () => {
    setActiveId(null);
    setMessages([]);
    setError(null);
  };

  const selectConversation = async (id: number) => {
    if (id === activeId) return;
    setActiveId(id);
    setError(null);
    setLoading(true);
    try {
      const detail = await getConversation(id);
      setMessages(detail.messages.map((m) => ({ role: m.role, content: m.content })));
    } catch (e) {
      setError(e instanceof Error ? e.message : "Could not load this conversation.");
      setMessages([]);
    } finally {
      setLoading(false);
    }
  };

  const handleSend = async (message: string) => {
    setError(null);
    setSending(true);
    setMessages((prev) => [...prev, { role: "user", content: message }]);

    const response = await sendChatMessage(message, activeId ?? undefined);
    setSending(false);

    if (!response.ok || !response.data) {
      if (response.errorCode === "QUOTA_EXCEEDED") {
        setQuotaReached(true);
      }
      setError(response.error ?? "Something went wrong. Please try again.");

      setMessages((prev) => prev.slice(0, -1));
      return;
    }

    const data = response.data;
    setMessages((prev) => [...prev, { role: "assistant", content: data.reply }]);
    setActiveId(data.conversationId);
    setUsage((prev) => ({
      ...prev,
      callsUsed: data.callsUsed,
      callsRemaining: data.callsRemaining,
    }));


    setConversations((prev) => {

      const summary: ConversationSummary = {
        id: data.conversationId,
        title: data.title,
        updatedDate: new Date().toISOString(),
      };

      return [summary, ...prev.filter((c) => c.id !== data.conversationId)];
    });
  };

  const handleDelete = async (id: number) => {
    const response = await deleteConversation(id);
    if (!response.ok) {
      setError(response.error ?? "Could not delete this conversation.");
      return;
    }
    setConversations((prev) => prev.filter((c) => c.id !== id));
    if (id === activeId) {
      startNewChat();
    }
  };

  return (
    <Box
      sx={{
        display: "flex",
        height: "calc(100vh - 140px)",
        border: "1px solid",
        borderColor: "divider",
        borderRadius: 2,
        overflow: "hidden",
      }}
    >
      <ChatSidebar
        usage={usage}
        conversations={conversations}
        activeId={activeId}
        onNewChat={startNewChat}
        selectConversation={selectConversation}
        onDelete={handleDelete}
      />
      <ChatBot
        messages={messages}
        sending={sending}
        loading={loading}
        error={error}
        quotaReached={quotaReached}
        onSend={handleSend}
      />
    </Box>
  );
}
