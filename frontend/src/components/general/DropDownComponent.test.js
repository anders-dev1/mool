import DropDown from "./DropDownComponent";
import {ThemeProvider} from "styled-components";
import {testTheme} from "./__test-utilities__/TestTheme";
import {fireEvent, render, screen} from "@testing-library/react";

const testOptions = [
    {label: "First", value: "first value"},
    {label: "Second", value: "second value"},
    {label: "Third", value: "third value"}
];

test('when in initial state, renders correctly', () => {
   const {container} = render(
       <ThemeProvider theme={testTheme}>
         <DropDown options={testOptions} initiallySelectedIndex={0} placeholder={"test placeholder"}/>
       </ThemeProvider>
   );

   expect(container).toMatchSnapshot('Default state');
});

test('when dropped down, renders correctly', () => {
   // Arrange
   const {container} = render(
       <ThemeProvider theme={testTheme}>
          <DropDown options={testOptions} initiallySelectedIndex={0}/>
       </ThemeProvider>
   );

   const dropDown = container.querySelector('div').firstChild;

   // Act
   fireEvent.click(dropDown);

   // Assert
   expect(container).toMatchSnapshot('Dropped down');
});

test('when initiallySelectedIndex provided, will set selectedIndex to what was provided', () => {
    // Arrange
    const index = 2;
    render(
        <ThemeProvider theme={testTheme}>
            <DropDown options={testOptions} initiallySelectedIndex={index}/>
        </ThemeProvider>
    );

    // Assert
    const option = screen.getByText(testOptions[index].label);
    expect(option).toBeDefined();
});

describe('option clicked', () =>{
    test('when option is not the one currently selected, should change selected index', () => {
        // Arrange
        const initiallySelectedIndex = 0;
        const indexToSelect = 1;
        const {container} = render(
            <ThemeProvider theme={testTheme}>
                <DropDown options={testOptions} initiallySelectedIndex={initiallySelectedIndex}/>
            </ThemeProvider>
        );

        // Act
        const dropDown = container.querySelector('div').firstChild;
        fireEvent.click(dropDown);
        const optionToSelect = screen.getByText(testOptions[indexToSelect].label);
        fireEvent.click(optionToSelect);

        // Assert
        // At this point the dropdown should have collapsed and only the newly selected option should be visible.
        const option = screen.getByText(testOptions[indexToSelect].label);
        expect(option).toBeDefined();
    });

    test('when option is not the one currently selected and changeEvent is provided, should call the changeEvent with option', () => {
        // Arrange
        const mockCallBack = jest.fn();
        const initiallySelectedIndex = 0;
        const indexToSelect = 1;
        const {container} = render(
            <ThemeProvider theme={testTheme}>
                <DropDown options={testOptions} initiallySelectedIndex={initiallySelectedIndex} changedEvent={mockCallBack}/>
            </ThemeProvider>
        );

        // Act
        const dropDown = container.querySelector('div').firstChild;
        fireEvent.click(dropDown);
        const optionToSelect = screen.getByText(testOptions[indexToSelect].label);
        fireEvent.click(optionToSelect);

        // Assert
        expect(mockCallBack).toHaveBeenCalledWith(testOptions[indexToSelect]);
    });

    test('when option is the same as the current index, should not call changeEvent', () => {
        // Arrange
        const mockCallBack = jest.fn();
        const initiallySelectedIndex = 0;
        const {container} = render(
            <ThemeProvider theme={testTheme}>
                <DropDown options={testOptions} initiallySelectedIndex={initiallySelectedIndex} changedEvent={mockCallBack}/>
            </ThemeProvider>
        );

        // Act
        const dropDown = container.querySelector('div').firstChild;
        fireEvent.click(dropDown);
        const optionToSelect = screen.getAllByText(testOptions[initiallySelectedIndex].label)[1];
        fireEvent.click(optionToSelect);

        // Assert
        expect(mockCallBack).not.toHaveBeenCalled();
    });
});