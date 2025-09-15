import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { SettingsService } from '../services/settings.service';
import { map, catchError, of } from 'rxjs';

export function passwordValidator(settingsService: SettingsService): AsyncValidatorFn {
  return (control: AbstractControl) => {
    if (!control.value) return of(null);

    return settingsService.validatePassword(control.value).pipe(
      map((result) => {
        return result.isValid ? null : { passwordPolicy: result.errors };
      }),
      catchError(() => of(null)) // nếu lỗi API thì bỏ qua validator
    );
  };
}
