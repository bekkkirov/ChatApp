import {Component, OnInit} from '@angular/core';
import {AuthService} from "./core/services/auth.service";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
    title = 'Client';

    constructor(private authService: AuthService) {
    }

    ngOnInit(): void {
        let tokens = localStorage.getItem("tokens");

        if(tokens) {
            this.authService.setAuthorizationStatus(true);
        }
    }
}
