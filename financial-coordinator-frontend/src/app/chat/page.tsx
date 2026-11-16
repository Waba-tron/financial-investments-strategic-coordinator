import { getUsage } from "@/actions/chat";
import { getConversations } from "@/actions/conversations";
import CheckoutBanner from "@/components/chat/CheckoutBanner";
import ChatWorkspace from "@/components/chat/ChatWorkspace";

// Protected route (enforced by middleware); chat requires authentication.
export default async function ChatPage({
  searchParams,
}: {
  searchParams: Promise<{ subscribed?: string }>;
}) {
  const [{ subscribed }, initialUsage, initialConversations] = await Promise.all([
    searchParams,
    getUsage(),
    getConversations(),
  ]);

  const status = subscribed === "true" ? "success" : subscribed === "false" ? "canceled" : null;

  return (
    <>
      {status && <CheckoutBanner status={status} />}
      <ChatWorkspace initialUsage={initialUsage} initialConversations={initialConversations} />
    </>
  );
}
