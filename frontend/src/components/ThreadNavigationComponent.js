import React from "react";
import {withTheme} from "styled-components";
import {ThreadCreationComponent} from "./general/ThreadCreationComponent";
import Divider from "./general/DividerComponent";
import {connect} from "react-redux";
import {threadActions} from "../redux/actions/threads.action";
import Spinner from "./general/SpinnerComponent";
import {ThreadComponent} from "./general/ThreadComponent";

class ThreadNavigationComponent extends React.PureComponent {
    componentDidMount() {
        this.props.updateThreads();
    }

    render() {
        return(
            <div className={this.props.className}>
                <ThreadCreationComponent/>
                <Divider className={"mt2"}/>

                { this.props.requestingThreads &&
                <div className={"flex justify-center mb2 mt2"}>
                    <Spinner size={"4"} color={this.props.theme.primaryColor}/>
                </div>}

                {
                    this.props.threadIds?.map(threadId => (
                        <ThreadComponent key={threadId} threadId={threadId} className={"mt2"}/>
                    ))
                }
                <div className={"mb2"}/>
            </div>
        );
    }
}

function mapState(state) {
    const { requestingThreads, threadIds } = state.threadsReducer;
    return { requestingThreads, threadIds };
}

const actionCreators = {
    updateThreads: threadActions.updateThreads
};

const connectedComponent = withTheme(connect(mapState, actionCreators)(ThreadNavigationComponent));
export {connectedComponent as ThreadNavigationComponent};
export * from './ThreadNavigationComponent'