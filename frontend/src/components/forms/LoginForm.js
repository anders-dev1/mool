import {Formik} from "formik";
import Input from "../general/InputComponent";
import Button from "../general/ButtonComponent";
import React from "react";
import styled, {withTheme} from "styled-components";
import * as Yup from "yup";
import InputError from "../general/InputErrorComponent";
import { connect } from 'react-redux';
import {sessionActions} from "../../redux/actions/session.action";
import SubmitError from "../general/SubmitErrorComponent";
import Alert from "../general/AlertComponent";

const Form = styled.form`
    font-size: 1em;
    display: flex;
    flex-direction: column;
`

class LoginForm extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            hideTokenExpired: false
        }
    }

    handleSubmit(values) {
        this.props.login(values.email, values.password);
    }

    render(){
        return(
            <div>
                {this.props.renewTokenExpired && this.state.hideTokenExpired === false &&
                <Alert
                    hideCallback={() => this.setState({hideTokenExpired: true})}
                    className={"mb1"}
                    header={"Your session timed out"}
                    text={"Your session timed out and you will have to log in again, sorry for the inconvenience."}/>
                }

                <Formik
                    initialValues={{ email: '', password: '' }}
                    options={{abortEarly: false}}
                    validationSchema={Yup.object().shape({
                        email: Yup.string()
                            .email(<InputError>Invalid email address</InputError>)
                            .required(<InputError>Required</InputError>),
                        password: Yup.string()
                            .min(8, <InputError>Must be more than 8 characters</InputError>)
                            .required(<InputError>Required</InputError>)
                    })}
                    onSubmit={(values, { setSubmitting }) => {
                        this.handleSubmit(values);
                    }}
                >
                    {({
                          values,
                          errors,
                          touched,
                          handleChange,
                          handleBlur,
                          handleSubmit,
                          isSubmitting
                      }) => (
                        <Form onSubmit={handleSubmit}>
                            <Input
                                data-testid="username"
                                placeholder="Email"
                                type="email"
                                name="email"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.email}
                            />
                            {touched.email && errors.email}
                            <Input
                                className={"mt1"}
                                data-testid="password"
                                placeholder="Password"
                                type="password"
                                name="password"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.password}
                            />
                            {errors.password && touched.password && errors.password}
                            <Button
                                data-testid="submit"
                                meaty
                                primary
                                type="submit"
                                className={"mt1"}
                                processing={this.props.loggingIn}>
                                Log in
                            </Button>
                        </Form>
                    )}
                </Formik>
                {this.props.loginFailure &&
                    <SubmitError>{this.props.loginFailure}</SubmitError>
                }
                {this.props.loginError &&
                    <Alert
                        hideCallback={() => this.props.hideLoginError()}
                        className={"mt1"}
                        color={"error"}
                        header={"An error has occured on the server"}
                        text={"We have been notified and will look into fixing this issue. We are sorry for the inconvenience."}/>
                }
            </div>
        )
    }
}

function mapState(state) {
    const { loginFailure, loggingIn, loginError, renewTokenExpired } = state.login;
    return { loginFailure, loggingIn, loginError, renewTokenExpired }
}

const actionCreators = {
    login: sessionActions.login,
    hideLoginError: sessionActions.hideLoginError
};
const connectedLoginForm = withTheme(connect(mapState, actionCreators)(LoginForm));
export {connectedLoginForm as LoginForm};
export * from './LoginForm'