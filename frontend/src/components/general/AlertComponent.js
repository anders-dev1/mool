import React from "react";
import styled, {withTheme} from 'styled-components';

const Container = styled.div`
  display: flex;
  justify-content: flex-start;
  align-items: flex-start;
  text-align: left;
  flex-direction: column;
  
  background-color: ${props => {
      switch (props.color){
        case "primary":
            return props.theme.primaryColor;
        case "success":
            return props.theme.successColor;
        case "error":
            return props.theme.errorColor;
        default:
            return props.theme.primaryColor;
      }
  }};
  border-radius: 5px;
  padding: 0.5em;
`

const Header = styled.p`
  font-size: 18px;
  color: white;
`

const Text = styled.p`
  margin-top: 0.5em;
  color: white;
`

const CloseButton = styled.svg`
  width: 16px;
  height: 16px;
  fill: white;
  cursor: pointer;
`

class Alert extends React.Component {
    render() {
        return(
            <Container
                className={this.props.className}
                color={this.props.color}>
                <div className={"flex fill"}>
                    <Header>{this.props.header}</Header>
                    <CloseButton
                        className={"ml-auto"}
                        onClick={() => this.props.hideCallback?.()}
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        viewBox="0 0 24 24">
                        <path  d="M12 2c5.514 0 10 4.486 10 10s-4.486 10-10 10-10-4.486-10-10 4.486-10 10-10zm0-2c-6.627 0-12 5.373-12 12s5.373 12 12 12 12-5.373 12-12-5.373-12-12-12zm6 16.094l-4.157-4.104 4.1-4.141-1.849-1.849-4.105 4.159-4.156-4.102-1.833 1.834 4.161 4.12-4.104 4.157 1.834 1.832 4.118-4.159 4.143 4.102 1.848-1.849z" />
                    </CloseButton>
                </div>

                <Text>{this.props.text}</Text>
            </Container>
        )
    }
}

export default withTheme(Alert)