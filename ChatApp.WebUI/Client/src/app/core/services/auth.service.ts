import {Inject, Injectable} from '@angular/core';
import {SignInData} from "../models/sign-in-data";
import {SignUpData} from "../models/sign-up-data";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {Tokens} from "../models/tokens";
import {API_BASE_URL} from "../extensions/injection-tokens";
import {map, ReplaySubject} from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private isLoggedInSource = new ReplaySubject<boolean>(1)
    isLoggedIn$ = this.isLoggedInSource.asObservable();

    constructor(
        @Inject(API_BASE_URL) private apiUrl: string,
        private http: HttpClient,
        private router: Router) {
    }

    setAuthorizationStatus(status: boolean) {
        this.isLoggedInSource.next(status);
    }

    saveTokens(tokens: Tokens) {
        localStorage.setItem("tokens", JSON.stringify(tokens));
    }

    signIn(data: SignInData) {
        return this.http.post<Tokens>(this.apiUrl + "auth/sign-in", data).pipe(
            map((response: Tokens) => {
                    this.setAuthorizationStatus(true);
                    this.saveTokens(response);
            })
        );
    }

    signUp(data: SignUpData) {
        return this.http.post<Tokens>(this.apiUrl + "auth/sign-up", data).pipe(
            map((response: Tokens) => {
                    this.setAuthorizationStatus(true);
                    this.saveTokens(response);
            })
        );
    }

    logout() {
        this.setAuthorizationStatus(false);
        localStorage.removeItem("tokens");

        this.router.navigateByUrl('/');
    }
}
