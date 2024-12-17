import { Component } from '@angular/core';
import { ShortenUrlComponent } from './shorten-url/shorten-url.component';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { UrlsTableComponent } from './urls-table/urls-table.component';
import { ShortenedUrlDto } from '../../models/shortened-url-dto.model';
import { UrlShortenerService } from '../../services/url-shortener.service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [ShortenUrlComponent, CommonModule, UrlsTableComponent, RouterLink],
  providers: [AuthService],
  templateUrl: './home.component.html',
  standalone: true
})
export class HomeComponent {
  isAuthenticated = false;
  shortenedUrls: ShortenedUrlDto[] = [];
  feedbackMessage: string | null = null;

  constructor(private authService: AuthService,
              private urlShortenerService: UrlShortenerService,
              private router: Router) {
    this.isAuthenticated = this.authService.getIsAuthenticated();
    try {
      this.urlShortenerService.getAll().then(x => {
        if (x !== undefined) this.shortenedUrls = x;
      });
    } catch (error) {
      console.log('Error getting urls:', error);
    }
  }

  addNewUrl(url: ShortenedUrlDto) {
    this.shortenedUrls.push(url);
  }

  deleteShortUrl(id: number) {
    this.shortenedUrls = this.shortenedUrls.filter(x => x.id !== id);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
