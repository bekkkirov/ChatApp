import {AbstractControl} from "@angular/forms";

export class PasswordValidators {
    static confirmPasswords(control: AbstractControl): { passwordsDontMatch: boolean } | null {
        if ((control.get('confirmPassword')?.value === control.get('password')?.value)) {
            return null;
        }
        
        return {passwordsDontMatch: true};
    }
}
