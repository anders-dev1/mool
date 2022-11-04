import fetchIntercept, {FetchInterceptorResponse} from 'fetch-intercept';
import {RenewToken} from "./api-token-renewer";

class Request {
    url: string;
    config: Config;
    constructor(url: string, config: Config) {
        this.url = url;
        this.config = config;
    }
}

const ignoredPaths: string[] = [
    "/api/accesstoken/renew",
    "/api/accesstoken/authenticate"
]

export class ApiBase {
    constructor() {
        let originalRequest: Request;
        const interceptor = fetchIntercept.register({
            request(url: string, config: Config): Promise<any[]> | any[] {
                if (ignoredPaths.some((path) => url.endsWith(path))){
                    // Calling this function removes this interceptor from the list of interceptors.
                    // This stops our interceptor from processing any longer in the current request chain.
                    interceptor();
                    return [url, config];
                }

                originalRequest = new Request(url, config);

                const token = localStorage.getItem('access-token');
                if (!token){
                    return [url, config];
                }

                const myHeaders: HeadersInit = new Headers();
                myHeaders.set('Accept', 'application/json');
                myHeaders.set('Content-Type', 'application/json');
                myHeaders.set('Authorization', "Bearer " + token);

                config.headers = myHeaders;

                return [url, config];
            },
            response(response: FetchInterceptorResponse): Promise<FetchInterceptorResponse> | any {
                if (response.status != 401) {
                    return response;
                }

                if (originalRequest.url.includes('token')) {
                    return response;
                }

                let refreshToken = localStorage.getItem("refresh-token");
                if (!refreshToken) {
                    return response;
                }

                // use refresh token to get new access token
                return RenewToken().then(
                    result => {
                        const accessToken = localStorage.getItem('access-token');

                        const myHeaders: HeadersInit = new Headers();
                        myHeaders.set('Authorization', "Bearer " + accessToken);
                        myHeaders.set('Accept', 'application/json');
                        myHeaders.set('Content-Type', 'application/json');
                        originalRequest.config.headers = myHeaders;

                        return fetch(originalRequest.url, originalRequest.config).then(
                            result => {
                                return result;
                            }
                        );
                    }
                );
            }
        });
    }
}

export interface Config {
    method: string;
    headers: HeadersInit;
}