import {AccessTokenClient} from "../../api";
import {AuthenticationQuery} from "../../api";
import {sessionConstants} from "../actiontypes";

export const sessionActions = {
    login,
    hideLoginError,
    renewTokenExpired,
    logout
};

function login(username, password) {
    return dispatch => {
        dispatch(request());

        let client = new AccessTokenClient();
        let testRequest = new AuthenticationQuery();
        testRequest.email = username;
        testRequest.password = password;
        client.authenticate(testRequest)
            .then(
                result => {
                    dispatch(success(result.tokens));
                },
                error => {
                    if (error.status === 404){
                        dispatch(failure("We couldn't find a Mool account with that information."));
                        return;
                    }
                    else if (error.status === 401){
                        dispatch(failure("The provided password was not correct."));
                        return;
                    }

                    dispatch(serverError());
                }
            );
    }

    function request() { return { type: sessionConstants.LOGIN_REQUEST } }
    function success(tokens) { return {type: sessionConstants.LOGIN_SUCCESS, tokens}}
    function failure(error) { return { type: sessionConstants.LOGIN_FAILURE, error } }
    function serverError() { return { type: sessionConstants.LOGIN_ERROR } }
}

function hideLoginError() {
    return dispatch => dispatch({type: sessionConstants.LOGIN_ERROR_HIDE});
}

function renewTokenExpired(){
    return dispatch => dispatch({type: sessionConstants.RENEW_TOKEN_EXPIRED});
}

function logout() {
    return dispatch => dispatch({type: sessionConstants.LOG_OUT});
}