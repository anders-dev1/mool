import styled, {css, withTheme} from "styled-components";

const SubmitError = styled.p`
    margin: 0.5em;
    text-align: center;

    ${props => props.theme.errorColor && css`
        color: ${props.theme.errorColor};
    `};
`

export default withTheme(SubmitError);