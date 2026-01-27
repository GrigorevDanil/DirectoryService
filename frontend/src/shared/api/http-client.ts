import axios, { isAxiosError } from "axios";
import { Envelope } from "./envelops";
import { EnvelopeError } from "./errors";

export const httpClient = axios.create({
  baseURL: process.env.NEXT_PUBLIC_DIRECTORY_SERVICE_URL,
  headers: {
    "Content-Type": "application/json",
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
