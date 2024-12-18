import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UrlShortenerService } from '../../services/url-shortener.service';

@Component({
  selector: 'app-redirect',
  imports: [],
  template: `
            <div class="flex justify-center items-center h-screen">
              <h1 [class.text-red-600]="isError" class="text-4xl p-5 bg-slate-50 rounded-lg">
                {{ text }}
              </h1>
            </div>
            `,
})
export class RedirectComponent implements OnInit {
  text: string = 'Redirecting...';
  isError: boolean = false;

  constructor(private route: ActivatedRoute,
              private urlShortenerService: UrlShortenerService,
  ) {}

  async ngOnInit(): Promise<void> {
    const shortCode = this.route.snapshot.paramMap.get('shortCode');
    if (shortCode) {
      try {
        const originalUrl = await this.urlShortenerService.getLongUrl(shortCode);
        window.location.href = originalUrl;
      } catch (e: any) {
        this.text = `Failed to redirect: ${e.error}`;
        this.isError = true;
        console.error(e);
      }
    } else {
      this.text = 'Invalid or missing URL code.';
      this.isError = true;
    }
  }
}

