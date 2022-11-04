import styled, {css, withTheme} from "styled-components";

const InputError = styled.p`
    margin: 0.5em 0 0 0.5em;
    text-align: left;

    ${props => props.theme.errorColor && css`
        color: ${props.theme.errorColor};
    `};
`

export default withTheme(InputError);