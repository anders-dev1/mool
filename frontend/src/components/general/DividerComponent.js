import styled, {css, withTheme} from 'styled-components';

const Divider = styled.div`
  width: 100%;
  height: 1px;
  
  ${props => css `
    background-color: ${props.theme.dividerColor};
  `};
`

export default withTheme(Divider);