import React from "react";
import ContentContainer from "./ContentContainer";
import ProfilePictureComponent from "./ProfilePictureComponent";
import Button from "./ButtonComponent";
import TextArea from "./TextAreaComponent";
import {Formik} from "formik";
import * as Yup from "yup";
import InputError from "./InputErrorComponent";
import {ThreadClient, ThreadCreationCommand} from "../../api";
import {threadActions} from "../../redux/actions/threads.action";
import {connect} from "react-redux";

class ThreadCreationComponent extends React.Component {
    constructor(props) {
        super(props);

        this.state = {isProcessingSubmit: false}
    }

    handleSubmit(values) {
        let client = new ThreadClient();
        let request = new ThreadCreationCommand();
        request.content = values.content;

        this.setState({
            isProcessingSubmit: true
        });

        client.create(request).then(
            result => {
                this.props.updateThreads();
            },
            error => {
            }
        );

        this.setState({
            isProcessingSubmit: false
        });
    }

    render(){
        return(
            <ContentContainer>
                <div className={"flex justify-start items-center"}>
                    <ProfilePictureComponent className="mr2 mb1"/>
                    <p>What is on your mind?</p>
                </div>

                <Formik
                    initialValues={{content:''}}
                    validationSchema={Yup.object().shape({
                        content: Yup.string()
                            .min(10, <InputError>Must be more than 10 characters</InputError>)
                            .required(<InputError>Required</InputError>)
                    })}
                    onSubmit={(values, { resetForm }) => {
                        this.handleSubmit(values);
                        resetForm();
                    }}
                >
                    {({
                          values,
                          errors,
                          touched,
                          handleChange,
                          handleBlur,
                          handleSubmit,
                          isSubmitting}) => (
                      <form onSubmit={handleSubmit}>
                          <TextArea
                              name={"content"}
                              data-testid={"threadCreationTextArea"}
                              onChange={handleChange}
                              onBlur={handleBlur}
                              value={values.content}
                              placeholder={"Write your thoughts here"}
                              className={"mb1"}/>
                          {errors.content && touched.content && errors.content}
                          <div className={"flex justify-end"}>
                              <Button
                                  data-testid={"submitThreadBtn"}
                                  type="submit"
                                  primary
                                  processing={this.state.isProcessingSubmit}
                                  className={"border"}>
                                  Post
                              </Button>
                          </div>
                      </form>
                    )}
                </Formik>

            </ContentContainer>
        );
    }
}

const actionCreators = {
    updateThreads: threadActions.updateThreads
};

const connectedService = connect(null, actionCreators)(ThreadCreationComponent);
export {connectedService as ThreadCreationComponent};
export * from './ThreadCreationComponent';