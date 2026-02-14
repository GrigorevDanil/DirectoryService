"use client";

import { PositionListActive } from "@/features/positions/list/position-list-active";
import { PositionListPageSize } from "@/features/positions/list/position-list-page-size";
import { PositionListSearch } from "@/features/positions/list/position-list-search";
import { PositionListSelectDepartments } from "@/features/positions/list/position-list-select-departments";
import { PositionListSortBy } from "@/features/positions/list/position-list-sort-by";
import { PositionListSortDirection } from "@/features/positions/list/position-list-sort-direction";
import { PositionCreateDialog } from "@/widgets/positions/position-create-dialog";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { PositionList } from "@/widgets/positions/position-list";

const { Container, Content, Filters, Header } = ListLayout;

export const PositionsPage = () => {
  return (
    <ListLayout className="p-2">
      <Header>
        <PositionListSearch />
        <PositionCreateDialog />
      </Header>
      <Container>
        <Content>
          <PositionList />
        </Content>
        <Filters className="w-75">
          <h3 className="font-medium text-sm mb-3">Фильтры</h3>
          <div className="space-y-3">
            <PositionListSortBy />
            <PositionListSortDirection />
            <PositionListActive />
            <PositionListPageSize />
            <PositionListSelectDepartments />
          </div>
        </Filters>
      </Container>
    </ListLayout>
  );
};
