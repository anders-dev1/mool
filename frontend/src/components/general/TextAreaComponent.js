import React from "react";
import TextareaAutosize from 'react-textarea-autosize';
import styled from "styled-components";

const OuterDiv = styled.div`
  width: 100%;
  height: 100%;
  
  textarea {
    display: block;
    font-size: 18px;
    padding: 0.5em;
    
    width: 100%;
    min-height: 48px;
    border-color: ${props => props.theme.inputColor};
    overflow: auto;

    box-sizing: border-box;
    
    outline: none;
    box-shadow: none;
    resize: none;
    border-radius: 5px;
  }
`

class TextArea extends React.Component {
    render(){
        return(
            <OuterDiv className={this.props.className}>
                <TextareaAutosize
                    data-testid={this.props['data-testid']}
                    placeholder={this.props.placeholder}
                    name={this.props.name}
                    onChange={this.props.onChange}
                    onBlur={this.props.onBlur}
                    value={this.props.value}/>
            </OuterDiv>
        );
    }
}

export default TextArea