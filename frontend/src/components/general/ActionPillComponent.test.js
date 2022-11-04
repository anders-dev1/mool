import renderer from 'react-test-renderer'
import ActionPill from "./ActionPillComponent";
import {fireEvent, render} from "@testing-library/react";
import {ThemeProvider} from "styled-components";
import {testTheme} from "./__test-utilities__/TestTheme";

test('renders correctly', () => {
    const tree = renderer
        .create(
            <ThemeProvider theme={testTheme}>
                <ActionPill className={"test"} link={"test.com"} color={"blue"} number={42} icon={"❤"}/>
            </ThemeProvider>)
        .toJSON();
    expect(tree).toMatchSnapshot();
});

test("triggers passed onClick event when clicked", () => {
    // Arrange
    const mockCallBack = jest.fn();
    const {container} = render(
        <ThemeProvider theme={testTheme}>
            <ActionPill onClick={mockCallBack}/>
        </ThemeProvider>);
    const component = container.querySelector('div');

    // Act
    fireEvent.click(component);

    // Assert
    expect(mockCallBack).toHaveBeenCalled();
});

test("cursor becomes pointer on hover", () => {
    // Arrange
    const {container} = render(
        <ThemeProvider theme={testTheme}>
            <ActionPill/>
        </ThemeProvider>);
    const component = container.querySelector('div');

    // Assert
    // The only way i know how to test this right now.
    expect(component).toHaveStyle("cursor: pointer;");
});

test("default color should be primary in theme", () => {
    // Arrange
    const {container} = render(
        <ThemeProvider theme={testTheme}>
            <ActionPill/>
        </ThemeProvider>);

    // Assert
    const component = container.querySelector('div');
    expect(component).toHaveStyle(`background-color: ${testTheme.primaryColor};`);
});

test("should use the given color", () => {
   // Arrange
    const {container} = render(
        <ThemeProvider theme={testTheme}>
            <ActionPill color={"red"}/>
        </ThemeProvider>);

    // Assert
    const component = container.querySelector('div');
    expect(component).toHaveStyle(`background-color: red;`);
});

test("should show a primary color spinner when provided isProcessing is true", () => {
    // Arrange
    const {container} = render(
        <ThemeProvider theme={testTheme}>
            <ActionPill processing={true}/>
        </ThemeProvider>
    )

    // Assert
    const spinnerContainerDiv = container.getElementsByClassName('lds-ring')[0];
    const spinnerPiece = spinnerContainerDiv.childNodes[0];
    expect(spinnerPiece).toHaveStyle(`border-color: ${testTheme.primaryColor} transparent transparent transparent`);
});