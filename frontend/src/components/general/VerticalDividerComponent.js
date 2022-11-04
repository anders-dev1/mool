import styled, {css, withTheme} from "styled-components";

const VerticalDivider = styled.div`
  width: 1px;
  background-color: #007BFF;
  
  ${props => css`
    height: ${props.height};
  `};

  ${props => css `
    background-color: ${props.theme.dividerColor};
  `};
`

export default withTheme(VerticalDivider);