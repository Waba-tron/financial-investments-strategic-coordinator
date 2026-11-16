"use client";

import { type UsageResponse } from "@/actions/chat";
import { type ConversationSummary } from "@/actions/conversations";
import SubscriptionActions from "@/components/chat/ChatSubscriptionBanner";
import AddIcon from "@mui/icons-material/Add";
import ChatBubbleOutlineIcon from "@mui/icons-material/ChatBubbleOutline";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import {
  Box,
  Button,
  Chip,
  Divider,
  IconButton,
  LinearProgress,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Stack,
  Typography,
} from "@mui/material";

interface ChatSidebarProps {
  usage: UsageResponse;
  conversations: ConversationSummary[];
  activeId: number | null;
  onNewChat: () => void;
  selectConversation: (id: number) => void;
  onDelete: (id: number) => void;
}

export default function ChatSidebar({
  usage,
  conversations,
  activeId,
  onNewChat,
  selectConversation,
  onDelete,
}: ChatSidebarProps) {
  const isPro = usage.plan === "Pro";
  const remaining = usage.callsRemaining ?? 0;
  const freeLimit = usage.callsUsed + remaining; // backend doesn't send the cap, so derive it
  const usedFraction = freeLimit > 0 ? usage.callsUsed / freeLimit : 0;

  return (
    <Box
      sx={{
        width: 280,
        flexShrink: 0,
        display: "flex",
        flexDirection: "column",
        height: "100%",
        borderRight: "1px solid",
        borderColor: "divider",
        bgcolor: "grey.50",
      }}
    >
      <Box sx={{ p: 2 }}>
        <Button fullWidth variant="contained" startIcon={<AddIcon />} onClick={onNewChat}>
          New chat
        </Button>
      </Box>

      <Divider />

      <Box sx={{ flex: 1, overflowY: "auto", px: 1, py: 1 }}>
        {conversations.length === 0 ? (
          <Typography
            variant="body2"
            color="text.secondary"
            sx={{ px: 2, py: 3, textAlign: "center" }}
          >
            No conversations yet.
          </Typography>
        ) : (
          <List dense disablePadding>
            {conversations.map((chat) => (
              <ListItemButton
                key={chat.id}
                selected={chat.id === activeId}
                onClick={() => selectConversation(chat.id)}
                sx={{ borderRadius: 1, pr: 1, "&:hover .delete-btn": { opacity: 1 } }}
              >
                <ListItemIcon sx={{ minWidth: 36 }}>
                  <ChatBubbleOutlineIcon fontSize="small" />
                </ListItemIcon>
                <ListItemText primary={chat.title} primaryTypographyProps={{ noWrap: true }} />
                <IconButton
                  className="delete-btn"
                  size="small"
                  aria-label="Delete conversation"
                  sx={{ opacity: { xs: 1, md: 0 }, transition: "opacity 0.15s" }}
                  onClick={(e) => {
                    e.stopPropagation();
                    onDelete(chat.id);
                  }}
                >
                  <DeleteOutlineIcon fontSize="small" />
                </IconButton>
              </ListItemButton>
            ))}
          </List>
        )}
      </Box>

      <Divider />

      {/* Usage panel */}
      <Box sx={{ p: 2 }}>
        <Stack direction="row" alignItems="center" justifyContent="space-between" sx={{ mb: 1 }}>
          <Typography variant="subtitle2">Usage</Typography>
          <Chip size="small" color={isPro ? "success" : "default"} label={isPro ? "Pro" : "Free"} />
        </Stack>

        {isPro ? (
          <Typography variant="body2" color="text.secondary">
            Unlimited messages.
          </Typography>
        ) : (
          <>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
              {usage.callsUsed} of {freeLimit} free calls used
            </Typography>
            <LinearProgress
              variant="determinate"
              value={Math.min(100, usedFraction * 100)}
              color={remaining === 0 ? "error" : "primary"}
              sx={{ height: 6, borderRadius: 3 }}
            />
            <Typography variant="caption" color="text.secondary" sx={{ mt: 0.5, display: "block" }}>
              {remaining} call{remaining === 1 ? "" : "s"} remaining
            </Typography>
          </>
        )}

        <SubscriptionActions plan={usage.plan} />
      </Box>
    </Box>
  );
}
