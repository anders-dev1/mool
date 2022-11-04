import React from "react";
import styled, {withTheme} from "styled-components";
import Spinner from "./SpinnerComponent";

const Container = styled.div`
  background-color: ${props => props.color ? props.color :  props.theme.primaryColor};
  height: 32px;
  min-width: 64px;
  cursor: pointer;
  user-select: none;
  
  // Removes anchor styling.
  color: inherit;
  text-decoration: none;

  border-radius: 20px;
  
  display: flex;
  justify-content: flex-end;
  align-items: center;
`

const ActionPillIcon = styled.p`
  position: relative;
  margin: 0 3px 0 5px;
`

const NumberCircle = styled.div`
  min-width: 24px;  
  height: 24px;
  background-color: #FFFFFF;
  border-radius: 15px;
  
  margin-right: 5px;
  
  display: flex;
  justify-content: center;
  align-items: center;
`

const NumberText = styled.p`
  position: relative;
  top: 2px;
  color: #000000;
  margin: 0 2px;
`

class ActionPill extends React.Component {
    render() {
        return(
            <Container
                href={this.props.link ?? "#"}
                className={this.props.className}
                onClick={this.props.onClick}
                color={this.props.color}
                data-testid={this.props['data-testid']}>
                <ActionPillIcon>
                    {this.props.icon}
                </ActionPillIcon>
                <NumberCircle>
                    {this.props.processing &&
                        <Spinner size={0.8} color={this.props.theme.primaryColor}/>}
                    {(this.props.processing === undefined || this.props.processing === false) &&
                        <NumberText data-testid={"number"}>{this.props.number}</NumberText>}
                </NumberCircle>
            </Container>
        )
    }
}

export default withTheme(ActionPill);