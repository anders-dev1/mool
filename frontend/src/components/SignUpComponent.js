import React from "react";
import styled, {withTheme} from "styled-components";
import ColorSpan from "./general/ColorSpanComponent";
import {Formik} from "formik";
import * as Yup from "yup";
import InputError from "./general/InputErrorComponent";
import Input from "./general/InputComponent";
import Button from "./general/ButtonComponent";
import {UserClient, UserCreationCommand} from "../api";
import Divider from "./general/DividerComponent";
import Alert from "./general/AlertComponent";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
`

const Title = styled.h1`
  font-size: 240%;
  margin: 4em 0 0 0;

  text-align: center;
  @media (min-width: 1000px) {
    text-align: left;
  }
`

const ContentContainer = styled.div`
  display: flex;
  flex-direction: column;
  text-align: center;

  max-width: 30em;

  background-color: white;
  padding: 1em 1em 1.5em 1em;
  margin: 0.5em;

  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .1), 0 8px 16px rgba(0, 0, 0, .1);
`

const TermsText = styled.p`
  padding-top: 1em;
  font-size: 0.75em;
  color: #99A5B0;
`

const Form = styled.form`
  font-size: 1em;
  display: flex;
  flex-direction: column;
`

class SignUp extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            isProcessingSignup: false,
            userWasCreated: false,
            showServerError: false
        }
    }

    hideServerAlert() {
        this.setState({showServerError: false});
    }

    handleSubmit(values) {
        this.setState({
            isProcessingSignup: true,
            showServerError: false
        });

        let client = new UserClient();
        let request = new UserCreationCommand();
        request.firstName = values.firstName;
        request.lastName = values.lastName;
        request.email = values.email;
        request.password = values.password;

        client.create(request).then(
            result => {
                this.setState({
                    isProcessingSignup: false,
                    userWasCreated: true
                });
            },
            error => {
                this.setState({
                    isProcessingSignup: false,
                    showServerError: true
                });
            }
        );
    }

    render() {
        return (
            <Container>
                <ColorSpan color={this.props.theme.primaryColor}>
                    <Title>Sign up</Title>
                </ColorSpan>
                <ContentContainer>
                    {this.state.userWasCreated === false &&
                    <div>
                        <Formik
                            initialValues={{firstName: '', lastName: '', email: '', password: '', confirm_password: ''}}
                            options={{abortEarly: false}}
                            validationSchema={Yup.object().shape({
                                firstName: Yup.string()
                                    .required(<InputError>Required</InputError>),
                                lastName: Yup.string()
                                    .required(<InputError>Required</InputError>),
                                email: Yup.string()
                                    .email(<InputError>Invalid email address</InputError>)
                                    .required(<InputError>Required</InputError>),
                                password: Yup.string()
                                    .min(8, <InputError>Must be more than 8 characters</InputError>)
                                    .required(<InputError>Required</InputError>),
                                confirm_password: Yup.string()
                                    .oneOf([Yup.ref('password'), null], <InputError>Passwords must match</InputError>)
                            })}
                            onSubmit={(values, {setSubmitting}) => {
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
                                    <div className={"mb1 flex"}>
                                        <div class={"fill mr1"}>
                                            <Input
                                                data-testid="firstName"
                                                placeholder="First name"
                                                type="text"
                                                name="firstName"
                                                onChange={handleChange}
                                                onBlur={handleBlur}
                                                value={values.firstName}
                                            />
                                            {touched.firstName && errors.firstName}
                                        </div>

                                        <div class={"fill ml1"}>
                                            <Input
                                                data-testid="lastName"
                                                placeholder="Last name"
                                                type="text"
                                                name="lastName"
                                                onChange={handleChange}
                                                onBlur={handleBlur}
                                                value={values.lastName}
                                            />
                                            {touched.lastName && errors.lastName}
                                        </div>
                                    </div>
                                    <Input
                                        data-testid="email"
                                        placeholder="Email"
                                        type="email"
                                        name="email"
                                        onChange={handleChange}
                                        onBlur={handleBlur}
                                        value={values.email}
                                    />
                                    {touched.email && errors.email}
                                    <Divider className={"mt2 mb2"}/>
                                    <Input
                                        data-testid="password"
                                        placeholder="Password"
                                        type="password"
                                        name="password"
                                        onChange={handleChange}
                                        onBlur={handleBlur}
                                        value={values.password}
                                    />
                                    {errors.password && touched.password && errors.password}
                                    <Input
                                        className={"mt1"}
                                        data-testid="repeat-password"
                                        placeholder="Confirm password"
                                        type="password"
                                        name="confirm_password"
                                        onChange={handleChange}
                                        onBlur={handleBlur}
                                        value={values.confirm_password}
                                    />
                                    {errors.confirm_password && touched.confirm_password && errors.confirm_password}
                                    <Divider className={"mt2"}/>

                                    {this.state.showServerError &&
                                        <Alert
                                            hideCallback={() => this.hideServerAlert()}
                                            show={this.state.showServerError}
                                            className={"mt2"}
                                            color={"error"}
                                            header={"An error has occured on the server"}
                                            text={"We have been notified and will look into fixing this issue. We are sorry for the inconvenience."}>
                                        </Alert>
                                    }

                                    <Button
                                        className={"mx-auto mt1"}
                                        data-testid={"signupsubmit"}
                                        type="submit"
                                        processing={this.state.isProcessingSignup}
                                        success
                                        limit
                                        center
                                        meaty>
                                        Sign up
                                    </Button>
                                </Form>
                            )}
                        </Formik>
                        <TermsText>By clicking "sign up" you agree that this is a pretty cool design, no
                            take-backsies.<br/> Also that you are pretty cool.</TermsText>
                    </div>
                    }

                    {this.state.userWasCreated === true &&
                    <div>
                        <p>Your account was created and can now be used for signing in. Click the button below to get
                            redirected to the sign-in page.</p>
                        <Button data-testid={"gotosignin"} link={"/"} className={"mt1 mx-auto"} primary limit center meaty>Go to sign-in</Button>
                    </div>
                    }
                </ContentContainer>
            </Container>
        );
    }
}

export default withTheme(SignUp);