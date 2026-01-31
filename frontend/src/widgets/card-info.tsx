import { Info, LucideIcon } from "lucide-react";
import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
} from "../shared/components/ui/card";
import { Fragment } from "react/jsx-runtime";
import { Separator } from "@/shared/components/ui/separator";

export interface CardInfoItemProps {
  icon: LucideIcon;
  title: string;
  value: string;
}

interface CardInfoProps extends React.ComponentProps<"div"> {
  items: CardInfoItemProps[];
}

export const CardInfo = ({ items }: CardInfoProps) => {
  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Info />
          Информация
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="space-y-2">
          {items.map((item, index) => {
            const isLast = index === items.length - 1;

            return (
              <Fragment key={index}>
                <CardInfoItem item={item} />

                {!isLast && <Separator />}
              </Fragment>
            );
          })}
        </div>
      </CardContent>
    </Card>
  );
};

const CardInfoItem = ({ item }: { item: CardInfoItemProps }) => {
  return (
    <div className="space-y-2">
      <div className="flex items-center gap-2 text-sm text-muted-foreground">
        <item.icon className="h-4 w-4" />
        <span>{item.title}</span>
      </div>
      <p className="font-medium text-sm">{item.value}</p>
    </div>
  );
};
