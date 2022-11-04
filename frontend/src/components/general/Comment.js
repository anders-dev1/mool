import React from "react";
import ProfilePictureComponent from "./ProfilePictureComponent";
import ActionPill from "./ActionPillComponent";
import {withTheme} from "styled-components";
import {connect} from "react-redux";
import {commentActions} from "../../redux/actions/comment.actions";

class Comment extends React.PureComponent {
    render() {
        return(
        <div>
            <div className={"flex justify-start"}>
                <ProfilePictureComponent className={"mr1"} isComment/>
                <div className={"flex-column pt1"}>
                    <p className={"sub"}>{this.props.comment.authorName}</p>
                    <p className={"sub mt1"} data-testid={"commentContent"}>{this.props.comment.content}</p>
                </div>
            </div>
            <div className={"flex justify-end"}>
                <ActionPill
                    icon={"👍"}
                    onClick={() => this.props.toggleCommentLike(this.props.commentId)}
                    number={this.props.comment.likes}
                    color={(this.props.comment.likedByCurrentUser
                        ? this.props.theme.successColor
                        : this.props.theme.primaryColor )}
                    className={"mr1"}
                    processing={this.props.comment.isChangingLikeStatus}
                    data-testid={"commentLikeBtn"}/>
            </div>
        </div>)
    }
}

function mapState(state, props) {
    const comment = state.commentsReducer.commentsById[props.commentId];
    return { comment }
}

const actionCreators = {
    toggleCommentLike: commentActions.toggleCommentLike
}

const connectedComponent = withTheme(connect(mapState, actionCreators)(Comment))
export {connectedComponent as Comment}
export * from './Comment';