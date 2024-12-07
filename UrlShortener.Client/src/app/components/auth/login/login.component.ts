import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['../auth.css'],
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, CommonModule, RouterLink],
  providers: [AuthService],
  standalone: true
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    if (authService.getIsAuthenticated()) this.router.navigate(['/']);

    this.loginForm = this.fb.group({
      login: new FormControl<string>('', [
        Validators.required,
        Validators.email
      ]),
      password: new FormControl<string>('', [
        Validators.required,
        Validators.minLength(8)
      ])
    });
  }

  async onSubmit() {
    if (!this.loginForm.valid) {
      return;
    }
    try {
      await this.authService.login(this.loginForm.value);
      this.router.navigate(['/']);
    } catch (errorInfo: any) {
      this.errorMessage = errorInfo.error?.message || 'An error occurred while logging in.';
    }
  }
}
