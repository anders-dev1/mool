import { sessionConstants } from "../actiontypes";

export function login(state = { loggedIn: localStorage.getItem('access-token') !== null }, action) {
    switch (action.type) {
        case sessionConstants.LOGIN_REQUEST:
            return {
                loggingIn: true,
                loginError: false,
                loginFailure: null,
                renewTokenExpired: false
            };
        case sessionConstants.LOGIN_FAILURE:
            return {
                loggingIn: false,
                loginFailure: action.error
            };
        case sessionConstants.LOGIN_ERROR:
            return {
                loggingIn: false,
                loginFailure: null,
                loginError: true
            };
        case sessionConstants.LOGIN_ERROR_HIDE:
            return {
                loginError: false
            };
        case sessionConstants.LOGIN_SUCCESS:
            localStorage.setItem('access-token', action.tokens.accessToken);
            localStorage.setItem('refresh-token', action.tokens.refreshToken);

            return {
                loggingIn: false,
                loggedIn: true,
            };
        case sessionConstants.LOG_OUT:
            localStorage.removeItem('access-token');
            localStorage.removeItem('refresh-token');

            return {
                loggedIn: false
            }
        case sessionConstants.RENEW_TOKEN_EXPIRED:
            localStorage.removeItem('access-token');
            localStorage.removeItem('refresh-token');

            return {
                renewTokenExpired: true,
                loggedIn: false
            };
        default:
            return state
    }
}