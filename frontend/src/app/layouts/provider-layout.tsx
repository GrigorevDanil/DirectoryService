"use client";

import { queryClient } from "@/shared/api/query-client";
import { QueryClientProvider } from "@tanstack/react-query";

export const ProviderLayout = ({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) => {
  return (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
};
