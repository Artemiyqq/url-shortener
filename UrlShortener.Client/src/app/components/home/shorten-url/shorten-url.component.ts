import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { UrlShortenerService } from '../../../services/url-shortener.service';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { ShortenedUrlDto } from '../../../models/shortened-url-dto.model';

@Component({
  selector: 'app-shorten-url',
  imports: [ReactiveFormsModule, MatFormFieldModule, CommonModule, MatInputModule],
  providers: [UrlShortenerService],
  templateUrl: './shorten-url.component.html',
  standalone: true
})
export class ShortenUrlComponent {
  @Output() urlShortened = new EventEmitter<ShortenedUrlDto>();

  shortenUrlForm: FormGroup;

  constructor(private fb: FormBuilder, private urlShortenerService: UrlShortenerService) {
    this.shortenUrlForm = new FormGroup({
      url: this.fb.control('', [Validators.required, Validators.pattern('https?://.+')])
    });
  }

  async onSubmit(fData: any, formDirective: FormGroupDirective): Promise<void> {
    if (!this.shortenUrlForm.valid) return;

    try {
      const newShortenedUrl = await this.urlShortenerService.shortenUrl(this.shortenUrlForm.value.url);
      this.urlShortened.emit(newShortenedUrl);
      formDirective.resetForm();
      this.shortenUrlForm.reset();
    } catch (error) {
      console.error('Error shortening URL:', error);
    }
  }
}