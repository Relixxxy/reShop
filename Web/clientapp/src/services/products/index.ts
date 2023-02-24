import axios, { AxiosInstance } from "axios";
import { httpClientService } from "services/httpClient";
import { config } from "setup/config";
import { Product } from "interfaces";

interface ProductsRequest {
  pageIndex: number;
  pageSize: number;
  filters: {
    type: string | null;
    brand: string | null;
  } | null;
}

interface ProductService {
  getProductsPaginated: (
    pageIndex: number,
    pageSize: number
  ) => Promise<Product[]>;
}

const axiosInstance: AxiosInstance = axios.create({
  baseURL: config.catalog_url,
  headers: {
    "Content-Type": "application/json",
  },
});

axiosInstance.interceptors.request.use(async (config) => {
  const user = await httpClientService.getUser();
  if (user) {
    config.headers["Authorization"] = `${user.token_type} ${user.access_token}`;
  }
  return config;
});

export const productService: ProductService = {
  getProductsPaginated: async (pageIndex, pageSize) => {
    const request: ProductsRequest = {
      pageIndex,
      pageSize,
      filters: null,
    };
    const response = await axiosInstance.post("items", request);
    return response.data;
  },
};
