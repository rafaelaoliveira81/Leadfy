import axios from "axios";

export const HTTPClient = axios.create({
  baseURL: "https://localhost:7287/api",
  headers: {
    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Headers": "Authorization",
    "Access-Control-Allow-Methods": "GET, POST, PUT, PATCH, DELETE, OPTIONS",
    "Content-Type": "application/json;charset=UTF-8",
  },
});
