import { httpClient } from "@/shared/api/http-client";
import { LocationDto } from "./types";
import { Envelope, PaginationEnvelope } from "@/shared/api/envelope";

export const locationsApi = {
  getLocations: async (): Promise<LocationDto[]> => {
    const response = await httpClient.get<
      Envelope<PaginationEnvelope<LocationDto>>
    >("/locations");

    return response.data.result?.items || [];
  },
};
