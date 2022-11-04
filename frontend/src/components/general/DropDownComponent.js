import React from "react";
import styled, {css, withTheme} from "styled-components";

const Dropper = styled.div`
  user-select: none;
  display: inline-block;
  position: relative;
`

const DropContainer = styled.div`
  cursor: pointer;
  width: 100%;
  flex-direction: column;
  //display: flex;
  
  padding-top: 0.25em;
  border-radius: 5px;

  &:hover {
    ${props => css `
      background-color: ${props.theme.backgroundColor};
    `};
  }
  
  ${props => props.droppedDown && css`
    background-color: ${props.theme.backgroundColor};
    border-radius: 5px 5px 0 0;
  `};
`

const DropTextAndArrowContainer = styled.div`
  margin: 0.25em 0.25em 0 0.25em;
  display: flex;
  width: 100%;
`

const OptionContainer = styled.div`
  padding: 0.25em 0.25em 0 0.25em;
  border-radius: 0 0 5px 5px;
  min-width: 100%;
  display: flex;
  flex-direction: column;
  position: absolute;
  z-index: 1;

  ${props => props.right && css`
    right: 0;
  `};
  
  ${props => css`
    background-color: ${props.theme.backgroundColor}
  `};
`

const Option = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  
  min-width: 14em;
  min-height: 2em;
  
  margin-bottom: 0.25em;
  
  background-color: white;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .1), 0 8px 16px rgba(0, 0, 0, .1);

  cursor: pointer;
  &:hover {
    ${props => css `
      background-color: ${props.theme.primaryHover};
      z-index: 1000;
    `};
  }

  ${props => props.selected && css`
    background-color: ${props.theme.primaryHover};
  `};
`

class DropDown extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            right: this.props.right !== undefined,
            droppedDown: false,
            options: this.props.options,
            selectedIndex: this.props.initiallySelectedIndex,
            placeholder: this.props.placeholder,
        }
    }

    selectOption(index) {
        if (this.state.selectedIndex === index)
            return;

        this.setState({selectedIndex: index});

        if (this.props.changedEvent !== undefined){
            this.props.changedEvent(this.state.options[index]);
        }
    }

    render() {
        return(
            <Dropper>
                <DropContainer
                    droppedDown={this.state.droppedDown}
                    onClick={() => this.setState({droppedDown: !this.state.droppedDown})}>
                    <DropTextAndArrowContainer>
                        <p>
                            {this.state.selectedIndex === undefined && this.state.placeholder}
                            {this.state.selectedIndex !== undefined && this.state.options[this.state.selectedIndex].label}
                        </p>
                        <img src={"expand.svg"} alt={"expand comments"} style={{transform: "translateY(-2px)"}}/>
                    </DropTextAndArrowContainer>

                    {this.state.droppedDown &&
                        <OptionContainer right={this.state.right}>
                            {this.state.options.map((option, index) => (
                                <Option key={index} selected={index === this.state.selectedIndex} onClick={() => this.selectOption(index)}>
                                    <p>{option.label}</p>
                                </Option>
                            ))}
                        </OptionContainer>
                    }
                </DropContainer>
            </Dropper>);
    }


}

export default withTheme(DropDown);