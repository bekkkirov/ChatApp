import {ErrorStateMatcher} from "@angular/material/core";
import {AbstractControl, FormGroupDirective, NgForm} from "@angular/forms";

export class ConfirmPasswordErrorStateMatcher implements ErrorStateMatcher {
    isErrorState(control: AbstractControl | null, form: FormGroupDirective | NgForm | null): boolean {
        if(control?.parent) {
            return control.parent.hasError('passwordsDontMatch') && control.parent.dirty;
        }

        return false;
    }

}