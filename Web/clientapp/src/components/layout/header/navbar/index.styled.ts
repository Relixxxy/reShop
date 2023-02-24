import styled from "styled-components";

export const NavbarList = styled.ul`
  display: flex;
  align-items: center;
  column-gap: 50px;
  padding: 0;

  list-style: none;
`;
export const NavbarItem = styled.li``;
export const NavbarLink = styled.a`
  color: #e6e6fa;
  font-size: 18px;
  text-decoration: none;
  transition: color 0.3s ease;
  &:hover {
    color: #9999ea;
  }
`;
