import { useCallback, useRef } from "react";

interface UseIntersectionObserverProps {
  hasNextPage: boolean;
  isFetchingNextPage: boolean;
  fetchNextPage: () => void;
}

export const useIntersectionObserver = ({
  hasNextPage,
  isFetchingNextPage,
  fetchNextPage,
}: UseIntersectionObserverProps) => {
  const observerRef = useRef<IntersectionObserver>(null);

  return useCallback(
    (node: HTMLDivElement | null) => {
      if (observerRef.current) {
        observerRef.current.disconnect();
      }

      if (!node || !hasNextPage) return;

      observerRef.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && !isFetchingNextPage) {
          fetchNextPage();
        }
      });

      observerRef.current.observe(node);
    },
    [hasNextPage, isFetchingNextPage, fetchNextPage],
  );
};
