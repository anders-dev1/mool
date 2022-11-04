import renderer from "react-test-renderer";
import {ThemeProvider} from "styled-components";
import {testTheme} from "./__test-utilities__/TestTheme";
import Alert from "./AlertComponent";
import {fireEvent, render} from "@testing-library/react";

test('renders correctly', () => {
    const tree = renderer.create(
        <ThemeProvider theme={testTheme}>
            <Alert
                className={"class"}
                header={"this is the header text"}
                text={"this is the alert text"}/>
        </ThemeProvider>).toJSON();
    expect(tree).toMatchSnapshot();
});

test("click on close button, when hideCallBack is supplied, should call hideCallBack", () =>{
    // Arrange
    const mockCallBack = jest.fn();
    const {container} = render(
        <ThemeProvider theme={testTheme}>
            <Alert hideCallback={mockCallBack}/>
        </ThemeProvider>);
    const closeButton = container.getElementsByClassName("ml-auto");

    // Act
    fireEvent.click(closeButton[0]);

    // Assert
    expect(mockCallBack).toHaveBeenCalled();
});

describe("when given color", () => {
    function renderWithColor(color) {
        const {container} = render(
            <ThemeProvider theme={testTheme}>
                <Alert color={color}/>
            </ThemeProvider>);

        return container.querySelector('div');
    }

    test("undefined, should display the primary theme color", () => {
        // Arrange
        const alert = renderWithColor();

        // Assert
        expect(alert).toHaveStyle(`background-color: ${testTheme.primaryColor};`);
    });

    test("primary, should display the primary theme color", () => {
        // Arrange
        const alert = renderWithColor("primary");

        // Assert
        expect(alert).toHaveStyle(`background-color: ${testTheme.primaryColor};`);
    });

    test("success, should display the primary theme color", () => {
        // Arrange
        const alert = renderWithColor("success");

        // Assert
        expect(alert).toHaveStyle(`background-color: ${testTheme.successColor};`);
    });

    test("error, should display the primary theme color", () => {
        // Arrange
        const alert = renderWithColor("error");

        // Assert
        expect(alert).toHaveStyle(`background-color: ${testTheme.errorColor};`);
    });
});