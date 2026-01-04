import axios from "axios";

export const httpClient = axios.create({
  baseURL: process.env.DIRECTORY_SERVICE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});
