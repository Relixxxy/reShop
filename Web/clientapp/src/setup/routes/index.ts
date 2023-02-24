import Home from "pages/home";
import Products from "pages/products";

import { FC } from "react";

interface Route {
  key: string;
  title: string;
  path: string;
  enabled: boolean;
  component: FC<{}>;
}

export const routes: Array<Route> = [
  {
    key: "home-route",
    title: "Home",
    path: "/",
    enabled: true,
    component: Home,
  },
  {
    key: "products-route",
    title: "Products",
    path: "/products",
    enabled: true,
    component: Products,
  },
];
