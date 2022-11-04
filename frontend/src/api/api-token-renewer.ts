import {AccessTokenClient, RenewAccessTokenCommand } from "./api-client";
import {store} from "../redux/store";
import {sessionActions} from "../redux/actions/session.action";

export async function RenewToken() {
    let refreshToken = localStorage.getItem("refresh-token");
    if (!refreshToken){
        return Promise.reject('There was no refresh token in storage.');
    }

    // use refresh token to get new access token
    let client = new AccessTokenClient();
    let request = new RenewAccessTokenCommand();
    request.refreshToken = refreshToken;

    await client.renew(request).then(
        result => {
            localStorage.setItem("access-token", result.accessToken);
        },
        error => {
            if (error.status === 404){
                store.dispatch(sessionActions.renewTokenExpired());
            }
        }
    );
}