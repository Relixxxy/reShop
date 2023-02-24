import { FC, ReactNode } from "react";
import Footer from "./footer";
import Header from "./header";
import { LayoutWrapper, Main } from "./index.styled";

interface LayoutProps {
  children: ReactNode;
}

const Layout: FC<LayoutProps> = ({ children }) => {
  return (
    <>
      <LayoutWrapper>
        <Header />
        <Main>{children}</Main>
        <Footer />
      </LayoutWrapper>
    </>
  );
};

export default Layout;
