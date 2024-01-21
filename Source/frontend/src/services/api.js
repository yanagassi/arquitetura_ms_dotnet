import axios from "axios";
import ConfigConstants from "../constants/config";

const api = axios.create({
  baseURL: "http://localhost:3001",
});

api.interceptors.request.use(async (config) => {
  const token = localStorage.getItem(ConfigConstants.tokenEnum);
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default api;
