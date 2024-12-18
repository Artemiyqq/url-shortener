import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener.service';
import { ShortenedUrlDto } from '../../../models/shortened-url-dto.model';
import { CommonModule } from '@angular/common';
import { ErrorToastComponent } from '../../error-toast/error-toast.component';

@Component({
  selector: 'app-urls-table',
  imports: [CommonModule, ErrorToastComponent],
  templateUrl: './urls-table.component.html',
})
export class UrlsTableComponent {
  @Input() shortenedUrls: ShortenedUrlDto[] = [];
  @Output() deleteShortUrl = new EventEmitter<number>();

  errorMessage: string | null = null;

  constructor(private urlShortenerService: UrlShortenerService) { }

  async onDelete(id: number) {
    try {
      await this.urlShortenerService.deleteByIndex(id);
      this.deleteShortUrl.emit(id);
    } catch (error: any) {
      if (error.status === 403){
        this.errorMessage = 'You are not authorized to delete this URL.';
        setTimeout(() => {
          this.errorMessage = null;
        }, 5000);
      }
    }
  }
}
