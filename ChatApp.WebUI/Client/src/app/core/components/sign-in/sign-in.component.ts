import {Component} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../services/auth.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";

@Component({
    selector: 'app-sign-in',
    templateUrl: './sign-in.component.html',
    styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
    form: FormGroup = new FormGroup({
        "userName": new FormControl(null, [Validators.required]),
        "password": new FormControl(null, [Validators.required]),
    });

    constructor(private authService: AuthService,
                private router: Router,
                private toastrService: ToastrService) {
    }

    signIn() {
        this.authService.signIn(this.form.value).subscribe({
                complete: () => this.router.navigateByUrl("/"),
                error: (err) => this.toastrService.error(err.error)
            }
        );
    }
}
