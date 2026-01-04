export interface LocationDto {
  id: string;
  name: string;
  timezone: string;
  address: AddressDto;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface AddressDto {
  country: string;
  postalCode: string;
  region: string;
  city: string;
  street: string;
  houseNumber: string;
}
