import { cn } from "@/shared/lib/utils";
import React from "react";

const ListLayout = ({
  children,
  className = "",
  ...props
}: React.ComponentProps<"div">) => {
  return (
    <div className={cn("flex flex-col gap-2", className)} {...props}>
      {children}
    </div>
  );
};

const Header = ({
  children,
  className = "",
  ...props
}: React.ComponentProps<"header">) => {
  return (
    <header className={cn("flex items-center gap-2", className)} {...props}>
      {children}
    </header>
  );
};

const Content = ({
  children,
  className = "",
  ...props
}: React.ComponentProps<"main">) => {
  return (
    <main
      className={cn("flex-1 min-w-0 overflow-auto pr-1", className)}
      {...props}
    >
      {children}
    </main>
  );
};

const Filters = ({
  children,
  className = "",
  ...props
}: React.ComponentProps<"aside">) => {
  return (
    <aside
      className={cn("border rounded-lg p-4 w-fit overflow-auto", className)}
      {...props}
    >
      {children}
    </aside>
  );
};

const Container = ({
  children,
  className = "",
  ...props
}: React.ComponentProps<"div">) => {
  return (
    <div
      className={cn("flex flex-1 overflow-hidden gap-2", className)}
      {...props}
    >
      {children}
    </div>
  );
};

ListLayout.Header = Header;
ListLayout.Content = Content;
ListLayout.Filters = Filters;
ListLayout.Container = Container;

export { ListLayout };
