import { createGlobalStyle, ThemeProvider } from 'styled-components';
import LandingPage from './components/LandingPageComponent';
import SignUp from "./components/SignUpComponent";
import {
    BrowserRouter as Router,
    Switch,
    Route
} from "react-router-dom"
import './App.css';
import {connect} from "react-redux";
import React from "react";
import {MessagesPageComponent} from "./components/MessagesPageComponent";
import Sandbox from "./components/SandboxComponent";

const lightTheme = {
    textColor: "black",
    primaryColor: "#007BFF",
    primaryHover: "rgba(0,123,255,0.33)",
    successColor: "#28A745",
    errorColor: "#F02849",
    backgroundColor: "#F0F2F5",
    buttonTextColor: "#FFFFFF",
    dividerColor: "#C7CBD1",
    inputColor: "#99A5B0",
    spinnerColor: "#FFFFFF transparent transparent transparent",
    font: "Acumin Pro, Arial, sans-serif !important;"
};

const GlobalStyle = createGlobalStyle`
  body {
    background-color: ${props => props.theme.backgroundColor};
  }
`

class App extends React.Component {
    render(){
        return(
            <ThemeProvider theme={lightTheme}>
                <GlobalStyle/>
                {this.props.loggedIn &&
                <Router>
                    <Switch>
                        <Route path="/sandbox">
                            <Sandbox/>
                        </Route>
                        <Route path="/">
                            <MessagesPageComponent/>
                        </Route>
                    </Switch>
                </Router>}

                {!this.props.loggedIn &&
                <Router>
                    <Switch>
                        <Route path="/signup">
                            <SignUp/>
                        </Route>
                        <Route path="/">
                            <LandingPage/>
                        </Route>
                    </Switch>
                </Router>}
            </ThemeProvider>
        )
    }
}

function mapState(state) {
    const { loggedIn } = state.login;
    return { loggedIn }
}

const connectedApp = connect(mapState, null)(App);
export { connectedApp as App };
export * from './App'