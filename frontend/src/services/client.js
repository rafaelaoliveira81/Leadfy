import axios from "axios";

export const HTTPClient = axios.create({
  baseURL: "https://localhost:7287/api",
  headers: {
    "Content-Type": "application/json;charset=UTF-8",
  },
});
