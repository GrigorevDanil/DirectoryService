"use client";

import { AlertCircle } from "lucide-react";
import { Button } from "@/shared/components/ui/button";

export const Error = ({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) => {
  return (
    <div className="min-h-screen bg-background">
      <div className="bg-destructive/10 border-b border-destructive/20 py-8 rounded-lg">
        <div className="mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex flex-col md:flex-row gap-8">
            <div className="w-24 h-24 rounded-full bg-destructive/15 flex items-center justify-center">
              <AlertCircle className="w-14 h-14 text-destructive" />
            </div>

            <div className="text-left flex-1">
              <h1 className="text-3xl sm:text-4xl font-bold text-foreground mb-4">
                Произошла ошибка
              </h1>

              <p className="text-lg text-muted-foreground mb-8 max-w-2xl">
                Что-то пошло не так при загрузке страницы.
              </p>

              <div className="space-y-6">
                <Button onClick={() => reset()} size="lg" className="min-w-48">
                  Попробовать еще раз
                </Button>

                {error.message && (
                  <div className="mt-6 bg-muted/50 border border-border rounded-lg p-5 w-full">
                    <p className="text-sm font-medium text-muted-foreground mb-2">
                      Техническая информация:
                    </p>
                    <p className="text-sm font-mono text-foreground break-all">
                      {error.message}
                    </p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
