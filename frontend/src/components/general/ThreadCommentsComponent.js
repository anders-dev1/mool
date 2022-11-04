import React from "react";
import Divider from "./DividerComponent";
import DropDown from "./DropDownComponent";
import {commentOrderOptions} from "./__shared__/commentOrderOptions";
import {withTheme} from "styled-components";
import {connect} from "react-redux";
import {commentActions} from "../../redux/actions/comment.actions";
import {Comment} from "./Comment";
import Spinner from "./SpinnerComponent";
import {CommentCreation} from "./CommentCreationComponent";

class ThreadCommentsComponent extends React.PureComponent {
    constructor(props) {
        super(props);

        this.commentOrderChanged = this.commentOrderChanged.bind(this);
    }

    commentOrderChanged(order) {
        this.props.changeThreadCommentOrder(this.props.threadId, order.value);
    }

    render() {
        return (
            <div className={"ml1 mr1"}>
                <Divider className={"mt1 mb2"}/>
                <CommentCreation threadId={this.props.threadId}/>

                {this.props.comments?.length > 0 &&
                    <div>
                        <Divider className={"mt1 mb2"}/>
                        <div className={"flex justify-end"}>
                            <DropDown
                                right
                                options={commentOrderOptions}
                                initiallySelectedIndex={this.props.thread.commentOrder}
                                changedEvent={this.commentOrderChanged}/>
                        </div>
                    </div>
                }

                {this.props.thread.fetchingComments &&
                    <div data-testid={"fetchingCommentsSpinner"} className={"flex justify-center"}>
                        <Spinner size={"4"} color={"#007BFF"}/>
                    </div>
                }

                {
                    this.props.comments?.map((comment, index) => (
                        <div key={comment.id}>
                            <Comment threadId={this.props.threadId} commentId={comment.id} comment={comment}/>
                            {index !== this.props.comments.length - 1 && <Divider className={"mt1 mb2"}/>}
                        </div>
                    ))
                }
            </div>
        );
    }
}

function mapState(state, ownProps) {
    const thread = state.threadsReducer.threadsById[ownProps.threadId] ?? undefined;

    // Find comments for thread.
    const commentIds = state.commentsReducer.threadIdToCommentIds[ownProps.threadId];
    const comments = [];
    if (commentIds !== undefined){
        commentIds.forEach(commentId => {
            comments.push(state.commentsReducer.commentsById[commentId]);
        });
    }

    return {
        thread,
        comments
    }
}

const actionCreators = {
    updateCommentsForThread: commentActions.updateCommentsForThread,
    changeThreadCommentOrder: commentActions.changeThreadCommentOrder,
    addCommentToThread: commentActions.addCommentToThread
}

const connectedComponent = withTheme(connect(mapState, actionCreators)(ThreadCommentsComponent));
export {connectedComponent as ThreadCommentsComponent};
export * from './ThreadCommentsComponent';