import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {DateValidators} from "../../validation/date-validators";
import {PasswordValidators} from "../../validation/password-validators";
import {ErrorStateMatcher} from "@angular/material/core";
import {ConfirmPasswordErrorStateMatcher} from "../../validation/confirm-password-error-state-matcher";

@Component({
    selector: 'app-sign-up',
    templateUrl: './sign-up.component.html',
    styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {
    form: FormGroup = new FormGroup({
        "userName": new FormControl(null, [Validators.required, Validators.minLength(5), Validators.maxLength(30)]),
        "email": new FormControl(null, [Validators.required, Validators.email]),
        "firstName": new FormControl(null, [Validators.required, Validators.minLength(2), Validators.maxLength(20)]),
        "lastName": new FormControl(null, [Validators.required, Validators.minLength(2), Validators.maxLength(20)]),
        "birthDate": new FormControl(null, [DateValidators.validDate]),
        "passwords": new FormGroup({
            "password": new FormControl(null, [Validators.required, Validators.minLength(5), Validators.maxLength(30)]),
            "confirmPassword": new FormControl(null)
        }, {validators: PasswordValidators.confirmPasswords})
    });

    matcher: ErrorStateMatcher = new ConfirmPasswordErrorStateMatcher();

    constructor() {
    }

    ngOnInit(): void {
    }
}