import { auth } from "@/auth";
import SignOutButton from "@/components/ui/SignOutButton";
import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import Link from "next/link";

export default async function NavBar() {
  const session = await auth();

  return (
    <AppBar position="static" color="primary">
      <Toolbar sx={{ gap: 2 }}>
        <Typography variant="h6" component={Link} href="/" sx={{ color: "inherit", textDecoration: "none", flexGrow: 1 }}>
          FISCo
        </Typography>
        {session && (
          <Button color="inherit" component={Link} href="/chat">Chat</Button>
        )}
        {session ? (
          <Box sx={{ display: "flex", alignItems: "center", gap: 2 }}>
            <Typography variant="body2">{session.user?.name}</Typography>
            <SignOutButton />
          </Box>
        ) : (
          <Button color="inherit" component={Link} href="/">Sign in</Button>
        )}
      </Toolbar>
    </AppBar>
  );
}
