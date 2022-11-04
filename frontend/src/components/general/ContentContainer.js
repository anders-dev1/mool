import styled, {withTheme} from "styled-components";

/*
    Main content container div used everywhere on the website.
 */
const ContentContainer = styled.div`
  padding: 0.5em;
  background-color: white;
  border-radius: 5px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, .1), 0 8px 16px rgba(0, 0, 0, .1);
`

export default withTheme(ContentContainer);