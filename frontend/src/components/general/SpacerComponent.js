import styled from "styled-components";

const handleSpacerHeight = height => {
    switch (height){
        case "half":
            return "0.5em";
        case "more":
            return "1.5em";
        case "double":
            return "2em";
        case "form":
            return "12em";
        default:
            return "1em"
    }
}

const handleSpacerWidth = width => {
    switch (width){
        case "half":
            return "0.5em";
        case "more":
            return "1.5em";
        case "double":
            return "2em";
        case "form":
            return "12em";
        default:
            return "1em"
    }
}


const Spacer = styled.div`
  height: ${({ height }) => handleSpacerHeight(height)};
  width: ${({ width }) => handleSpacerWidth(width)};
`

export default Spacer;