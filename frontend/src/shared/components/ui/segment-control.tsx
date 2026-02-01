"use client";

import * as React from "react";
import * as ToggleGroup from "@radix-ui/react-toggle-group";
import { motion } from "motion/react";
import { cn } from "@/shared/lib/utils";

type SegmentControlContextValue = {
  value?: string;
};

const SegmentControlContext =
  React.createContext<SegmentControlContextValue | null>(null);

function useSegmentControlContext() {
  const ctx = React.useContext(SegmentControlContext);
  if (!ctx) {
    throw new Error(
      "SegmentControl components must be used within <SegmentControl />",
    );
  }
  return ctx;
}

export type SegmentControlProps = Omit<
  ToggleGroup.ToggleGroupSingleProps,
  "type"
>;

function SegmentControl({
  className,
  value,
  defaultValue,
  onValueChange,
  children,
  ...props
}: SegmentControlProps) {
  return (
    <SegmentControlContext.Provider value={{ value }}>
      <ToggleGroup.Root
        type="single"
        value={value}
        defaultValue={defaultValue}
        onValueChange={onValueChange}
        className={cn(
          "relative isolate flex h-10 rounded-full bg-background p-1 ring-1 ring-border",
          className,
        )}
        {...props}
      >
        {children}
      </ToggleGroup.Root>
    </SegmentControlContext.Provider>
  );
}

type SegmentControlItemProps = ToggleGroup.ToggleGroupItemProps;

function SegmentControlItem({
  className,
  children,
  value,
  ...props
}: SegmentControlItemProps) {
  const { value: activeValue } = useSegmentControlContext();
  const isActive = activeValue === value;

  return (
    <ToggleGroup.Item
      value={value}
      className={cn(
        "relative h-8 w-8 rounded-full flex items-center justify-center",
        className,
      )}
      {...props}
    >
      {isActive && (
        <motion.div
          layoutId="segment-control-indicator"
          className="absolute inset-0 rounded-full bg-secondary"
          transition={{ type: "spring", duration: 0.5 }}
        />
      )}
      <span className="relative z-10">{children}</span>
    </ToggleGroup.Item>
  );
}

export { SegmentControl, SegmentControlItem };
