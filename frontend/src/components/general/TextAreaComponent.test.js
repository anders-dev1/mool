import {fireEvent, render, screen} from "@testing-library/react";
import TextArea from "./TextAreaComponent";

test('renders correctly', () => {
   const {container} = render(
       <TextArea name={"testname"} placeholder={"this is the placeholder"} value={"initial value"}/>
   );

   expect(container).toMatchSnapshot();
});

test('input changed, when onChange is provided, calls Callback', () =>{
   // Arrange
   const mockCallback = jest.fn();
   const {container} = render(<TextArea onChange={mockCallback}/>);
   const component = container.querySelector('textarea');

   // Act
   fireEvent.change(component, {target: {value: 'hello'}});

   // Assert
   expect(mockCallback).toBeCalled();
});

test('input left after being interacted with, when onBlur is provided, calls Callback', () =>{
   // Arrange
   const mockCallback = jest.fn();
   const {container} = render(<TextArea onBlur={mockCallback}/>);
   const component = container.querySelector('textarea');

   // Act
   fireEvent.focusOut(component);

   // Assert
   expect(mockCallback).toBeCalled();
});