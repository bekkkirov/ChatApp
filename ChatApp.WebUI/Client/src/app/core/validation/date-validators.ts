import * as moment from "moment";
import {AbstractControl} from "@angular/forms";

export class DateValidators {
    static validDate(control: AbstractControl): { invalidDate: boolean } | null {
        let date = moment(control?.value);

        if (date.isValid() && date >= moment("1900-01-01")) {
            return null;
        }

        return {invalidDate: true};
    }
}
