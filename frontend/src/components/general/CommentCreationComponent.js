import * as Yup from "yup";
import InputError from "./InputErrorComponent";
import {Form, Formik} from "formik";
import ProfilePictureComponent from "./ProfilePictureComponent";
import TextArea from "./TextAreaComponent";
import Button from "./ButtonComponent";
import React from "react";
import {commentActions} from "../../redux/actions/comment.actions";
import {withTheme} from "styled-components";
import {connect} from "react-redux";

class CommentCreation extends React.Component {
    render() {
        return (
            <Formik
                initialValues={{comment: ''}}
                validationSchema={Yup.object().shape({
                    comment: Yup.string()
                        .min(10, <InputError>Must be 10 or more characters</InputError>)
                        .required(<InputError>Required</InputError>)
                })}
                onSubmit={(values, {resetForm}) => {
                    this.props.addCommentToThread(this.props.threadId, values.comment);

                    // Would be better for the form to somehow subscribe to the success event and then reset.
                    // You could pass the resetForm as a callback to the action, but that
                    // ruins SoC by having the action dealing directly with UI components instead
                    // of through state.
                    resetForm();
                }}>
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
                        <div className={"flex items-start"}>
                            <ProfilePictureComponent isComment/>
                            <div className={"flex-auto ml1"}>
                                <TextArea
                                    name={"comment"}
                                    placeholder={"Write your input here..."}
                                    onChange={handleChange}
                                    onBlur={handleBlur}
                                    value={values.comment}
                                    data-testid={"commentCreationTextArea"}/>
                                {touched.comment && errors.comment}
                            </div>
                        </div>

                        <div className={"flex justify-end mt1"}>
                            <Button primary type="submit"
                                    processing={this.props.postingComment}
                                    data-testid={"commentCreationPostBtn"}>Post</Button>
                        </div>
                    </Form>
                )}
            </Formik>
        )
    }
}

function mapState(state, props) {
    const postingComment = state.threadsReducer.threadsById[props.threadId].postingComment;
    return {postingComment}
}

const actionCreators = {
    addCommentToThread: commentActions.addCommentToThread
}

const connectedComponent = withTheme(connect(mapState, actionCreators)(CommentCreation));
export {connectedComponent as CommentCreation};
export * from './CommentCreationComponent'