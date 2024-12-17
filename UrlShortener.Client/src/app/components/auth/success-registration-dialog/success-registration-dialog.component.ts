import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogActions, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-success-registration-dialog',
  imports: [MatDialogContent, MatDialogActions, MatButtonModule],
  template: `
    <p mat-dialog-title class="mb-0 pt-3 text-center text-2xl">Registration Successful!</p>
    <mat-dialog-content style="color: black; ">You can now log in or return to the home page.</mat-dialog-content>
    <mat-dialog-actions>
      <button class="w-1/3" (click)="goToLogin()">Log In</button>
      <button class="w-1/3" (click)="goToHome()">Home</button>
    </mat-dialog-actions>
  `,
  styles: [`
    h2 {
      text-align: center;
      margin-bottom: 0.5rem;
    }

    mat-dialog-actions {
      display: flex;
      justify-content: center;
      gap: 10px;
    }
  `]
})
export class SuccessRegistrationDialogComponent {
  constructor(private dialogRef: MatDialogRef<SuccessRegistrationDialogComponent>, private router: Router) { }

  goToLogin() {
    this.dialogRef.close();
    this.router.navigate(['/login']);
  }

  goToHome() {
    this.dialogRef.close();
    this.router.navigate(['']);
  }
}

