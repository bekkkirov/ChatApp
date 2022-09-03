import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {SignInComponent} from './core/components/sign-in/sign-in.component';
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {ReactiveFormsModule} from "@angular/forms";
import {SignUpComponent} from './core/components/sign-up/sign-up.component';
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import {AppRoutingModule} from "./core/routing/app-routing.module";
import {API_BASE_URL} from "./core/extensions/injection-tokens";
import {environment} from "../environments/environment";
import {HttpClientModule} from "@angular/common/http";
import {ToastrModule} from "ngx-toastr";

@NgModule({
    declarations: [
        AppComponent,
        SignInComponent,
        SignUpComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        ReactiveFormsModule,
        MatDatepickerModule,
        MatNativeDateModule,
        AppRoutingModule,
        HttpClientModule,
        ToastrModule.forRoot({positionClass: 'toast-bottom-right'}),
    ],
    providers: [
        {
            provide: API_BASE_URL,
            useValue: environment.apiBaseUrl
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
