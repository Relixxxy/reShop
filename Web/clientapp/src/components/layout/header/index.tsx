import { FC, ReactElement } from "react";
import { HeaderWrapper, Logo } from "./index.styled";
import Navbar from "./navbar";
import Login from "./login";

const Header: FC = (): ReactElement => {
  return (
    <HeaderWrapper>
      <Logo>reShop</Logo>
      <Navbar />
      <Login />
    </HeaderWrapper>
  );
};

export default Header;
