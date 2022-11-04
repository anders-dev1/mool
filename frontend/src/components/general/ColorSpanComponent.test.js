import renderer from "react-test-renderer";
import ColorSpan from "./ColorSpanComponent";
import {render} from "@testing-library/react";

test('renders correctly', () => {
   const tree = renderer
       .create(
           <ColorSpan>Hey</ColorSpan>
       ).toJSON();
   expect(tree).toMatchSnapshot();
});

describe('color', () => {
    test('when color is provided, uses color in span', () => {
        //Arrange
        const providedColor = "red";
        const {container} = render(
            <ColorSpan color={providedColor}/>
        );

        // Assert
        const component = container.querySelector('span');
        expect(component).toHaveStyle(`color: ${providedColor}`);
    });

    test('when color is not provided, uses black color', () => {
        //Arrange
        const {container} = render(
            <ColorSpan/>
        );

        // Assert
        const component = container.querySelector('span');
        expect(component).toHaveStyle(`color: black`);
    });
});
