import React from 'react'
import {render} from '@testing-library/react'
import {Provider} from 'react-redux'
import {setupStore, store} from "../../../redux/store";
import {testTheme} from "./TestTheme";
import {ThemeProvider} from "styled-components";

// This is used to set up the store for tests.
export function renderWithProviders(
    ui,
    {
        preloadedState = {},
        // Automatically create a store instance if no store was passed in
        testStore = setupStore(preloadedState),
        ...renderOptions
    } = {}
) {
    function Wrapper({children}) {
        return(
            <Provider store={testStore}>
                <ThemeProvider theme={testTheme}>
                {children}
                </ThemeProvider>
            </Provider>
        );
    }

    return {testStore, ...render(ui, {wrapper: Wrapper, ...renderOptions})}
}

// This is used with the react-test-renderer to render dom in tests.
export function internalsForRenderer(
    ui,
    {
        preloadedState = {},
        testStore = setupStore(preloadedState)
    } = {}
){
    function Wrapper(children) {
        return(
            <Provider store={testStore}>
                <ThemeProvider theme={testTheme}>
                    {children}
                </ThemeProvider>
            </Provider>
        );
    }

    return Wrapper(ui);
}