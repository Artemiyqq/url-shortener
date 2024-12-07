import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, FormControl, Validators, AbstractControl, ValidatorFn } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SuccessRegistrationDialogComponent } from '../success-registration-dialog/success-registration-dialog.component';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, CommonModule, RouterLink, MatDialogModule],
  providers: [AuthService],
  templateUrl: './register.component.html',
  styleUrls: ['../auth.css'],
  standalone: true
})
export class RegisterComponent {
  registerForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private dialog: MatDialog) {
    this.registerForm = this.fb.group({
      name: new FormControl<string>('', [
        Validators.required,
        Validators.minLength(3)
      ]),
      login: new FormControl<string>('', [
        Validators.required,
        Validators.email
      ]),
      password: new FormControl<string>('', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)
      ]),
      confirmPassword: new FormControl<string>('', Validators.required)
    });

    this.registerForm.get('confirmPassword')?.setValidators([
      Validators.required,
      this.passwordMismatchValidator(this.registerForm.get('password')!)
    ]);
  }

  passwordMismatchValidator(password: AbstractControl<any, any>): ValidatorFn {
    return (control: AbstractControl<any, any>) => {
      return control.value !== password.value ? { passwordMismatch: true } : null;
    };
  }

  async onSubmit() {
    if (!this.registerForm.valid) {
      return;
    }
    try {
      const { confirmPassword, ...registerDto } = this.registerForm.value;
      await this.authService.register(registerDto);
      this.dialog.open(SuccessRegistrationDialogComponent);
    } catch (errorInfo: any) {
      this.errorMessage = errorInfo.error.message;
    }
  }
}