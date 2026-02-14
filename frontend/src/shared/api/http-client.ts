import axios, { isAxiosError } from "axios";
import { Envelope } from "./envelops";
import { EnvelopeError } from "./errors";

export const API_V1 = "/v1";

export const httpClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
  paramsSerializer: {
    indexes: null,
  },
});

httpClient.interceptors.response.use(
  (response) => {
    const envelope = response.data as Envelope;

    if (
      envelope.isError &&
      envelope.errorList &&
      envelope.errorList.length > 0
    ) {
      throw new EnvelopeError(envelope.errorList);
    }

    return response;
  },
  (error) => {
    if (isAxiosError(error) && error.response?.data) {
      const envelope = error.response.data as Envelope;

      if (
        envelope.isError &&
        envelope.errorList &&
        envelope.errorList.length > 0
      ) {
        throw new EnvelopeError(envelope.errorList);
      }
    }

    return Promise.reject(error);
  },
);
