import { Directive, forwardRef } from '@angular/core';
import {
  NG_ASYNC_VALIDATORS,
  AsyncValidator,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { SettingsService } from '../services/settings.service';
import { passwordValidator } from './settings-validator';
@Directive({
  selector: '[appPasswordValidator][ngModel]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: forwardRef(() => PasswordValidatorDirective),
      multi: true,
    },
  ],
})
export class PasswordValidatorDirective implements AsyncValidator {
  constructor(private settingsService: SettingsService) {}

  validate(
    control: AbstractControl
  ): Promise<ValidationErrors | null> | import('rxjs').Observable<ValidationErrors | null> {
    return passwordValidator(this.settingsService)(control);
  }
}
