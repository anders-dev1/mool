import React from 'react';
import { withTheme} from 'styled-components';
import DropDown from "./general/DropDownComponent";
import ContentContainer from "./general/ContentContainer";

class Sandbox extends React.Component {
    options = [
        {label: "Latest comments", value: 1},
        {label: "Oldest comments", value: 2}
    ]

    selectChanged(selectedOption){
        console.log(selectedOption);
    }

    render() {
        return(
            <div>
                <ContentContainer className={"m4 p4"}>
                    <div>
                        <DropDown options={this.options} initiallySelectedIndex={0} changedEvent={this.selectChanged} right/>
                    </div>
                    <p>Tesasdfasdfasdft</p>
                    <p>Tesasdfasdfasdft</p>
                    <p>Tesasdfasdfasdft</p>
                </ContentContainer>
            </div>
        )
    }
}

export default withTheme(Sandbox);