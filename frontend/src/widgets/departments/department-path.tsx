import {
  Breadcrumb,
  BreadcrumbList,
  BreadcrumbItem,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/shared/components/ui/breadcrumb";
import { Fragment } from "react";

export const DepartmentPath = ({ path }: { path: string }) => {
  const parts = path.split(".");

  return (
    <div className="flex gap-2 items-center">
      <p>[</p>
      <Breadcrumb>
        <BreadcrumbList>
          {parts.map((part, index) => {
            const isLast = index === parts.length - 1;

            return (
              <Fragment key={index}>
                <BreadcrumbItem>
                  {isLast ? <BreadcrumbPage>{part}</BreadcrumbPage> : part}
                </BreadcrumbItem>

                {!isLast && <BreadcrumbSeparator />}
              </Fragment>
            );
          })}
        </BreadcrumbList>
      </Breadcrumb>
      <p>]</p>
    </div>
  );
};
