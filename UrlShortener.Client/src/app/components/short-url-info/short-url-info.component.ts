import { Component, OnInit } from '@angular/core';
import { ShortUrlInfoDto } from '../../models/short-url-info-dto.model';
import { UrlShortenerService } from '../../services/url-shortener.service';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-short-url-info',
  imports: [CommonModule, RouterModule],
  templateUrl: './short-url-info.component.html',
})
export class ShortUrlInfoComponent implements OnInit {
  shortUrlInfoData: ShortUrlInfoDto | null = null;
  errorMessage: string | null = null;

  constructor(private urlShortenerService: UrlShortenerService,
              private route: ActivatedRoute,
              public authService: AuthService
  ) {}

  async ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!this.initialChecks(id)) return;

    try{
      this.urlShortenerService.getShortUrlInfoDto(id!).then((data) => {
        this.shortUrlInfoData = data;
      });
    } catch (error: any) {
      this.errorMessage = error.error;
    }
  }

  initialChecks(id: string | null): boolean {
    if (!this.authService.getIsAuthenticated()) {
      this.errorMessage = 'You are not authorized to view this page.';
      return false;
    }

    if (id === null) {
      this.errorMessage = 'Invalid URL ID.';
      return false;
    }

    return true;
  }
}
