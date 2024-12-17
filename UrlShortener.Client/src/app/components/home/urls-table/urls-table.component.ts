import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UrlShortenerService } from '../../../services/url-shortener.service';
import { ShortenedUrlDto } from '../../../models/shortened-url-dto.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-urls-table',
  imports: [CommonModule],
  templateUrl: './urls-table.component.html',
})
export class UrlsTableComponent {
  @Input() shortenedUrls: ShortenedUrlDto[] = [];
  @Output() deleteShortUrl = new EventEmitter<number>();

  constructor(private urlShortenerService: UrlShortenerService) { }

  async onDelete(id: number) {
    try {
      await this.urlShortenerService.deleteByIndex(id);
      this.deleteShortUrl.emit(id);
    } catch (error) {
      console.error('Error deleting URL:', error);
    }
  }
}
