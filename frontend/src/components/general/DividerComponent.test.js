import renderer from "react-test-renderer";
import Divider from "./DividerComponent";
import {ThemeProvider} from "styled-components";
import {testTheme} from "./__test-utilities__/TestTheme";

test('renders correctly', () => {
   const tree = renderer.create(
       <ThemeProvider theme={testTheme}>
         <Divider/>
       </ThemeProvider>
   ).toJSON();
   expect(tree).toMatchSnapshot();
});