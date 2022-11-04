import React from 'react';
import { withTheme} from 'styled-components';
import styled from 'styled-components';
import Button from './general/ButtonComponent';
import Spacer from "./general/SpacerComponent";
import ColorSpan from "./general/ColorSpanComponent";
import Divider from "./general/DividerComponent";
import {LoginForm} from "./forms/LoginForm";

const PageContainer = styled.div`
    display: flex;
    justify-content: center;
`

const ContentContainer = styled.div`
    flex-basis: 30em;
    display: flex;

    flex-direction: column;
    @media (min-width: 1000px) {
        flex-basis: 60em;
        flex-direction: row;
        padding-top: 12em;
    }
`

const WelcomeMessageContainer = styled.div`
    padding-top: 1em;
    
    @media (min-width: 1000px) {
        padding-top: 2em;
    }
`

const WelcomeMessageTitle = styled.h1`
    margin: 0 0 0.2em;
    font-size: 240%;
    
    text-align: center;
    @media (min-width: 1000px) {
        text-align: left;
    }
`

const WelcomeMessageText = styled.p`
    
    font-size: 140%;

    text-align: center;
    @media (min-width: 1000px) {
        text-align: left;
        margin-bottom: 2em;
    }
`

const LoginContainer = styled.div`
    display: flex;
    flex-direction: column;
    justify-content: center;

    background-color: white;
    padding: 1em 1em 1.5em 1em;

    border-radius: 5px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, .1), 0 8px 16px rgba(0, 0, 0, .1);

    @media (min-width: 1000px) {
        flex-basis: 24em;
    }
`

class LandingPage extends React.Component {
    render() {
        return(
            <PageContainer>
                <ContentContainer>
                    <WelcomeMessageContainer>
                        <ColorSpan color={this.props.theme.primaryColor}>
                            <WelcomeMessageTitle>Mool Message Board</WelcomeMessageTitle>
                        </ColorSpan>
                        <WelcomeMessageText>
                            <ColorSpan color={this.props.theme.primaryColor}>Mool</ColorSpan> enables you to share your thoughts with the entire world.</WelcomeMessageText>
                        <WelcomeMessageText>Be kind 😊.</WelcomeMessageText>
                    </WelcomeMessageContainer>
                    <LoginContainer>
                        <LoginForm/>
                        <Spacer height="half"/>
                        <Divider/>
                        <Spacer height="more"/>
                        <Button
                            data-testid={"signup"}
                            link="/signup"
                            className={"mx-auto"}
                            success
                            meaty
                            limit>
                            Create new account
                        </Button>
                    </LoginContainer>
                </ContentContainer>
            </PageContainer>
        )
    }
}

export default withTheme(LandingPage);