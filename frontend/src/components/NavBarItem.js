import React from "react";
import styled, {css, withTheme} from "styled-components";

const Padding = styled.div`
  height: 100%;
  
  ${props => props.right && css`
    margin-left: auto;
  `};
`

const NavBarMenuItemContainer = styled.div`
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  position: relative;
`

const NavBarMenuItem = styled.p`
  font-size: 1.125em;
  padding: 0.25em;

  border-radius: 20px;
  &:hover {
    ${props => css `
      background-color: ${props.theme.backgroundColor} 
    `};
  }
  cursor: pointer;
  
  ${props => {
      if (props.selected){
        return css`color: ${props.theme.primaryColor}`;  
      } else{
        return css`color: ${props.theme.textColor}`;  
      }
  }};
  
`

const SelectedUnderline = styled.div`
  width: calc(100% - 0.71875em);
  height: 2px;
  position: absolute;
  bottom: 0;
  
  ${props => css `
    background-color: ${props.theme.primaryColor};
  `};
`

class NavBarItem extends React.Component {
    render() {
        return(
            <Padding
                right={this.props.right}
                onClick={() => this.props.onClick?.()}
                className={this.props.className}>
                <NavBarMenuItemContainer>
                    <NavBarMenuItem selected={this.props.selected}>
                        {this.props.children}
                    </NavBarMenuItem>
                    {this.props.selected && <SelectedUnderline/>}
                </NavBarMenuItemContainer>
            </Padding>
        );
    }
};

export default withTheme(NavBarItem);