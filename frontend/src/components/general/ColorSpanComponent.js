import styled from "styled-components";

const ColorSpan = styled.span`
  color: ${props => props.color ? props.color : "black"};
`

export default ColorSpan;