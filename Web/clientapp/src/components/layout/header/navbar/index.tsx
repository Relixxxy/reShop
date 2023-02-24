import { FC, ReactElement } from "react";
import { routes } from "setup/routes/index";
import { NavLink } from "react-router-dom";
import { NavbarList, NavbarItem, NavbarLink } from "./index.styled";

const Navbar: FC = (): ReactElement => {
  return (
    <NavbarList>
      {routes.map(
        (route) =>
          route.enabled && (
            <NavbarItem key={route.key}>
              <NavbarLink as={NavLink} to={route.path}>
                {route.title}
              </NavbarLink>
            </NavbarItem>
          )
      )}
    </NavbarList>
  );
};

export default Navbar;
