import renderer from "react-test-renderer";
import ContentContainer from "./ContentContainer";
import {testTheme} from "./__test-utilities__/TestTheme";
import {ThemeProvider} from "styled-components";

test('renders correctly', () => {
   const tree = renderer.create(
       <ThemeProvider theme={testTheme}>
         <ContentContainer>This is a test</ContentContainer>
       </ThemeProvider>
   ).toJSON();
   expect(tree).toMatchSnapshot();
});