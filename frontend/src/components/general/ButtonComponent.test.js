import Button from "./ButtonComponent";
import {fireEvent, render, screen} from "@testing-library/react";
import {testTheme} from "./__test-utilities__/TestTheme";
import {ThemeProvider} from "styled-components";
import {MemoryRouter, Route} from "react-router-dom";
import renderer from "react-test-renderer";

test('renders correctly', () => {
    const domTree = renderer
        .create(
            <ThemeProvider theme={testTheme}>
                <Button class={"classname"} meaty primary/>
            </ThemeProvider>
        ).toJSON()
    expect(domTree).toMatchSnapshot();
});

test('when provided processing value is true, spinner should show', () => {
    // Arrange
    render(
        <ThemeProvider theme={testTheme}>
            <Button processing/>
        </ThemeProvider>
    );

    // Assert
    const component = screen.getByTestId('spinner');
    expect(component).toHaveStyle('display: flex');
});

describe('button clicked', () => {
    test('when onClick callback is provided, should activate callback', () => {
        // Arrange
        const mockCallBack = jest.fn();
        const {container} = render(
            <ThemeProvider theme={testTheme}>
                <Button onClick={mockCallBack}/>
            </ThemeProvider>);
        const component = container.querySelector('button');

        // Act
        fireEvent.click(component);

        // Assert
        expect(mockCallBack).toHaveBeenCalled();
    });

    test('when link is provided, should route to link', () => {
        // Arrange
        const testPath = "test";
        let testLocation;

        const {container} = render(
            <ThemeProvider theme={testTheme}>
                <MemoryRouter>
                    <Button link={testPath}/>
                    <Route
                        path="*"
                        render={({location}) => {
                            testLocation = location;
                            return null;
                        }}/>
                </MemoryRouter>
            </ThemeProvider>);
        const component = container.querySelector('button');

        // Act
        fireEvent.click(component);

        // Assert
        expect(testLocation.pathname).toEqual(`/${testPath}`);
    });
});