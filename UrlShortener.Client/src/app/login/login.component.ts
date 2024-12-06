import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, CommonModule],
  providers: [AuthService],
  standalone: true
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.loginForm = this.fb.group({
      login: new FormControl<string>('', Validators.required),
      password: new FormControl<string>('', Validators.required)
    });
  }

  async onSubmit() {
    if (!this.loginForm.valid) return;
    try {
      const result = await this.authService.login(this.loginForm.value);
    } catch (errorInfo: any) {
      this.errorMessage = errorInfo.error.message;
    }
  }
}