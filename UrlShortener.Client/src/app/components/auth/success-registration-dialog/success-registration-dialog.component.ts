import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogActions, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-success-registration-dialog',
  imports: [MatDialogContent, MatDialogActions, MatButtonModule],
  template: `
    <h2 mat-dialog-title style="margin-bottom: 0rem">Registration Successful!</h2>
    <mat-dialog-content style="color: black">You can now log in or return to the home page.</mat-dialog-content>
    <mat-dialog-actions>
      <button (click)="goToLogin()">Log In</button>
      <button (click)="goToHome()">Home</button>
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

