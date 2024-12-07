import { Component } from '@angular/core';
import { ShortenUrlComponent } from './shorten-url/shorten-url.component';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  imports: [ShortenUrlComponent, CommonModule],
  providers: [AuthService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: true
})
export class HomeComponent {
  isAuthenticated = false;

  constructor(private authService: AuthService) {
    this.isAuthenticated = this.authService.getIsAuthenticated();
  }
}
