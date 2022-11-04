import React from "react";
import {withTheme} from "styled-components";
import ContentContainer from "./ContentContainer";
import ProfilePictureComponent from "./ProfilePictureComponent";
import ActionPill from "./ActionPillComponent";
import TimeAgo from "react-timeago";
import {connect} from "react-redux";
import {threadActions} from "../../redux/actions/threads.action";
import {ThreadCommentsComponent} from "./ThreadCommentsComponent";

class ThreadComponent extends React.PureComponent {
    render() {
        return (
            <ContentContainer className={this.props.className}>
                <div className={"flex justify-start"}>
                    <ProfilePictureComponent className="mr1 mb2"/>
                    <div className={"flex-column pt1"}>
                        <p>{this.props.thread.author}</p>
                        {this.props.isTest === undefined &&
                            <p className={"sm"}><TimeAgo date={this.props.thread.created}/></p>}
                    </div>
                </div>
                <p className={"mb1"} data-testid={"threadContent"} >{this.props.thread.content}</p>
                <div className={"flex justify-end"}>
                    <ActionPill
                        icon={"👍"}
                        onClick={() => this.props.toggleThreadLike(this.props.thread.id)}
                        number={this.props.thread.likes}
                        color={(this.props.thread.likedByCurrentUser
                            ? this.props.theme.successColor
                            : this.props.theme.primaryColor)}
                        className={"mr1"}
                        processing={this.props.thread.isChangingLikeStatus}
                        data-testid={"threadLikeBtn"}/>
                    <ActionPill
                        icon={"💬"}
                        number={this.props.thread.numberOfComments}
                        onClick={() => this.props.toggleCommentSection(this.props.thread.id)}
                        data-testid={"threadCommentBtn"}/>
                </div>

                {this.props.thread.showingCommentSection &&
                    <ThreadCommentsComponent threadId={this.props.thread.id}/>}
            </ContentContainer>
        )
    }
}

function mapState(state, props) {
    const thread = state.threadsReducer.threadsById[props.threadId];
    return {thread}
}

const actionCreators = {
    toggleThreadLike: threadActions.toggleThreadLike,
    toggleCommentSection: threadActions.toggleCommentSection
};

const connectedComponent = withTheme(connect(mapState, actionCreators)(ThreadComponent));
export {connectedComponent as ThreadComponent};
export * from './ThreadComponent';