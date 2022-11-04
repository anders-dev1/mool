import React from "react";
import styled, {css, withTheme} from 'styled-components';
import VerticalDivider from "./general/VerticalDividerComponent";
import NavBarItem from "./NavBarItem";
import MainContentArea from "./general/MainContentArea";
import {sessionActions} from "../redux/actions/session.action";
import { connect } from 'react-redux';
import axios from "axios";

const NavBar = styled.div`
  background-color: white;
  height: 4em;
  box-shadow: 0px 0px 10px rgba(0, 0, 0, .16);
  
  display:flex;
  align-items: center;
  justify-content: center;
`

const NavBarContent = styled.div`
  display: flex;
  align-items: center;
  height: 100%;
`

const NavBarTitle = styled.h1`
  font-size: 2.5em;
  margin-top: 0.75em;
  
  ${props => css`
    color: ${props.theme.primaryColor}
  `};
`

class NavBarComponent extends React.Component {
    logOut(){
        this.props.logout();
    }

    render() {
        return(
          <NavBar>
              <MainContentArea>
                  <NavBarContent>
                      <NavBarTitle className="sm-hide">Mool</NavBarTitle>
                      <NavBarTitle className="sm-only">M</NavBarTitle>
                      <VerticalDivider height="3em" className="sm-hide ml2"/>
                      <NavBarItem className={"ml2"} selected>Messages</NavBarItem>
                      <NavBarItem className={"ml1"}>Profile</NavBarItem>
                      <NavBarItem right onClick={() => this.logOut()}>Log out</NavBarItem>
                  </NavBarContent>
              </MainContentArea>
          </NavBar>
        );
    }
}

const actionCreators = {
    logout: sessionActions.logout
};

const connectedNavBar = withTheme(connect(null, actionCreators)(NavBarComponent));
export {connectedNavBar as NavBar}
export * from './NavBarComponent'