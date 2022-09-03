import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import {AuthService} from "../services/auth.service";

@Injectable({
  providedIn: 'root'
})
export class IsNotLoggedInGuard implements CanActivate {
    
    constructor(private authService: AuthService) {
    }

    canActivate(): boolean {
        let isLoggedIn: boolean = false;

        this.authService.isLoggedIn$.subscribe(result => {
            isLoggedIn = result;
        })

        return !isLoggedIn;
    }
  
}
