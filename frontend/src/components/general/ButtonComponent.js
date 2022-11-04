import React from "react";
import styled, {css, withTheme} from 'styled-components';
import {Link} from "react-router-dom";
import Spinner from "./SpinnerComponent";

const ButtonContainer = styled.div `
  text-align: center;
`

const ButtonStyle = styled.button`
  position: relative;
  padding: 0 2em;

  display: flex;
  justify-content: center;
  align-items: center;
  
  border: none;
  border-radius: 5px;
  
  // Meaty makes the button bigger. We do this by increasing font size and decreasing height respectively.
  // 2.5 * 1.2 = 3
  font-size: ${props => props.meaty ? '1.2em' : '1em' };
  height: ${props => props.meaty ? "2.5em" : "2em" };

  font-weight: bold;
  cursor: pointer;
  transition-duration: 0.4s;
  outline: none;
  
  ${props => props.theme.font && css`
    font-family: ${props.theme.font};
  `};
  ${props => props.primary && css`
    background: ${props.theme.primaryColor};
    color: ${props.theme.buttonTextColor};
  `};
  ${props => props.success && css`
    background: ${props.theme.successColor};
    color: ${props.theme.buttonTextColor};
  `};
`

const ButtonChildContainer = styled.div`
  opacity: ${props => props.processing ? "0" : "1" }
`

const ButtonSpinnerContainer = styled.div`
  height: 2em;
  position: absolute;
  margin: 0 auto;
  
  display: ${props => props.processing ? "flex" : "none" };
  justify-content: center;
  align-items: center;
`

const ButtonLink = styled(Link)`
  &:focus, &:hover, &:visited, &:link, &:active {
    text-decoration: none;
  }
`

class Button extends React.Component {
  render() {
    let btnElement =
      <ButtonStyle
          data-testid={this.props['data-testid']}
          className={this.props.className}
          meaty={this.props.meaty}
          primary={this.props.primary}
          success={this.props.success}
          onClick={this.props.onClick}
          type={this.props.type}>
        <ButtonChildContainer data-testid="childcontainer"
            processing={this.props.processing}>
          {this.props.children}
        </ButtonChildContainer>
        <ButtonSpinnerContainer
            data-testid="spinner"
            processing={this.props.processing}>
          <Spinner size={this.props.meaty ? "1.6" : "1.5"}/>
        </ButtonSpinnerContainer>
      </ButtonStyle>;

    if (this.props.link != null){
      btnElement = <ButtonLink to={this.props.link}>{btnElement}</ButtonLink>;
    }

    if (this.props.limit){
      btnElement = <ButtonContainer center={this.props.center} className={this.props.className}>{btnElement}</ButtonContainer>
    }

    return (btnElement)
  }
}

export default withTheme(Button);