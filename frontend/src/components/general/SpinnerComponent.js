import React from "react";
import './SpinnerComponent.css';
import styled, {withTheme} from 'styled-components';

const Container = styled.div`
  display: inline-block;
  width: ${props => props.size ? props.size + "em" : "2em"};
  height: ${props => props.size ? props.size + "em" : "2em"};
`

const SpinnerPiece = styled.div`
  box-sizing: border-box;
  display: block;
  position: absolute;
  width: ${props => props.size ? props.size + "em" : "2em"};
  height: ${props => props.size ? props.size + "em" : "2em"};
  border-width: ${ props => props.size ? props.size * 0.2 + "em" : "0.4em"};
  border-style: solid;
  border-radius: 50%;
  animation: lds-ring 1.2s cubic-bezier(0.5, 0, 0.5, 1) infinite;
  border-color: ${props => props.color !== undefined ? props.color + " transparent transparent transparent" : props.theme.spinnerColor};
`

class Spinner extends React.Component {
    render(){
        return(
        <Container size={this.props.size} className={`lds-ring`}>
            <SpinnerPiece size={this.props.size} color={this.props.color}/>
            <SpinnerPiece size={this.props.size} color={this.props.color}/>
            <SpinnerPiece size={this.props.size} color={this.props.color}/>
            <SpinnerPiece size={this.props.size} color={this.props.color}/>
        </Container>
        );
    }
}

export default withTheme(Spinner);