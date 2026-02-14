"use client";

import { LocationCreateDialog } from "@/widgets/locations/location-create-dialog";
import { LocationListActive } from "@/features/locations/list/location-list-active";
import { LocationListPageSize } from "@/features/locations/list/location-list-page-size";
import { LocationListSearch } from "@/features/locations/list/location-list-search";
import { LocationListSortBy } from "@/features/locations/list/location-list-sort-by";
import { LocationListSortDirection } from "@/features/locations/list/location-list-sort-direction";
import { LocationInfinityList } from "@/widgets/locations/location-infinity-list";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { LocationListSelectDepartments } from "@/features/locations/list/location-list-select-departments";

const { Container, Content, Filters, Header } = ListLayout;

export const LocationsPage = () => {
  return (
    <ListLayout className="p-2">
      <Header>
        <LocationListSearch />
        <LocationCreateDialog />
      </Header>
      <Container>
        <Content>
          <LocationInfinityList />
        </Content>
        <Filters className="w-75">
          <h3 className="font-medium text-sm mb-3">Фильтры</h3>
          <div className="space-y-3">
            <LocationListSortBy />
            <LocationListSortDirection />
            <LocationListActive />
            <LocationListPageSize />
            <LocationListSelectDepartments />
          </div>
        </Filters>
      </Container>
    </ListLayout>
  );
};
