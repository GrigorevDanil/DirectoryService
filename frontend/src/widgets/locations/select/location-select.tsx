"use client";

import { Spinner } from "@/shared/components/ui/spinner";
import { Error } from "@/widgets/error";
import { ListLayout } from "@/shared/components/ui/list-layout";
import { GetLocationsRequest } from "@/entities/locations/api";
import { LocationDto, LocationId } from "@/entities/locations/types";
import { LocationListId } from "@/entities/locations/model/location-list-store";
import { useLocationInfinityList } from "@/entities/locations/hooks/use-location-infinity-list";
import { LocationSelectCard } from "./location-select-card";
import { LocationSelected } from "./location-selected";
import { LocationListSearch } from "@/features/locations/list/location-list-search";
import { LocationListSortBy } from "@/features/locations/list/location-list-sort-by";
import { LocationListSortDirection } from "@/features/locations/list/location-list-sort-direction";
import { LocationListActive } from "@/features/locations/list/location-list-active";
import { LocationListPageSize } from "@/features/locations/list/location-list-page-size";
import { useMemo } from "react";

const { Header, Container, Content, Filters } = ListLayout;

interface LocationSelectProps extends React.ComponentProps<"div"> {
  selectedLocations: LocationDto[];
  onChangeChecked: (selectedLocations: LocationDto[]) => void;
  stateId: LocationListId;
  request?: GetLocationsRequest;
  multiselect?: boolean;
  excludeIds?: LocationId[];
}

export const LocationSelect = ({
  selectedLocations,
  onChangeChecked,
  stateId,
  className,
  request,
  multiselect = true,
  excludeIds = [],
  ...props
}: LocationSelectProps) => {
  const {
    locations: fetchedLocations,
    error,
    isFetching,
    isFetchingNextPage,
    isPending,
    refetch,
    cursorRef,
  } = useLocationInfinityList({ stateId, request });

  const locations = useMemo(() => {
    if (excludeIds.length === 0) {
      return fetchedLocations;
    }

    const excludeSet = new Set(excludeIds);
    return fetchedLocations.filter((d) => !excludeSet.has(d.id));
  }, [fetchedLocations, excludeIds]);

  const handleCheckedChange = (selected: boolean, location: LocationDto) => {
    if (multiselect) {
      if (selected) {
        onChangeChecked([...selectedLocations, location]);
      } else {
        onChangeChecked(
          selectedLocations.filter((loc) => loc.id !== location.id),
        );
      }
      return;
    }

    if (selected) {
      onChangeChecked([location]);
    } else {
      onChangeChecked([]);
    }
  };
  const handleRemoveLocation = (locationId: LocationId) => {
    onChangeChecked(selectedLocations.filter((loc) => loc.id !== locationId));
  };

  const isSelected = (locationId: LocationId) => {
    return selectedLocations.some((loc) => loc.id === locationId);
  };

  const selectedResolved = useMemo(() => {
    const byId = new Map(locations.map((l) => [l.id, l]));

    return selectedLocations.map((sel) => byId.get(sel.id) ?? sel);
  }, [locations, selectedLocations]);

  const renderContent = () => {
    if (isPending && locations.length === 0) {
      return (
        <div className="flex justify-center">
          <Spinner />
        </div>
      );
    }

    if (error) {
      return <Error error={error} reset={refetch} />;
    }

    if (locations.length === 0 && !isFetching) {
      return (
        <div className="text-center text-muted-foreground">
          Нет доступных локаций
        </div>
      );
    }

    return (
      <div className="flex flex-col gap-2 w-full">
        {locations.map((location) => (
          <LocationSelectCard
            key={location.id}
            location={location}
            checked={isSelected(location.id)}
            onCheckedChange={handleCheckedChange}
          />
        ))}
        <div ref={cursorRef} className="flex justify-center">
          {isFetchingNextPage && <Spinner />}
        </div>
      </div>
    );
  };

  return (
    <ListLayout className={className} {...props}>
      <LocationSelected
        selectedLocations={selectedResolved}
        onRemove={handleRemoveLocation}
      />
      <Header>
        <LocationListSearch stateId={stateId} />
      </Header>
      <Container>
        <Content className="min-w-75">{renderContent()}</Content>
        <Filters>
          <h3 className="font-medium text-sm mb-3">Фильтры</h3>
          <div className="space-y-3">
            <LocationListSortBy stateId={stateId} />
            <LocationListSortDirection stateId={stateId} />
            {!request?.isActive && <LocationListActive stateId={stateId} />}
            <LocationListPageSize stateId={stateId} />
          </div>
        </Filters>
      </Container>
    </ListLayout>
  );
};
