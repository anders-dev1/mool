import React from "react";
import {NavBar} from "./NavBarComponent";
import MainContentArea from "./general/MainContentArea";
import {ThreadNavigationComponent} from "./ThreadNavigationComponent";

export class MessagesPageComponent extends React.Component {
    render() {
        return (
            <div>
                <NavBar/>
                <MainContentArea>
                    <ThreadNavigationComponent className={"mt2"}/>
                </MainContentArea>
            </div>
        );
    }
}
