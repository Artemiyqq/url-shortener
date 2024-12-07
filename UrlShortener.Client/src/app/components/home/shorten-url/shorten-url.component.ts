import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { UrlShortenerService } from '../../../services/url-shortener.service';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-shorten-url',
  imports: [ReactiveFormsModule, MatFormFieldModule, CommonModule, MatInputModule ],
  providers: [UrlShortenerService],
  templateUrl: './shorten-url.component.html',
  styleUrl: './shorten-url.component.css',
  standalone: true
})
export class ShortenUrlComponent {
  shortenUrlForm: FormGroup;

  constructor(private fb: FormBuilder, private urlShortenerService: UrlShortenerService)
  {
    this.shortenUrlForm = new FormGroup({
      url: this.fb.control('', [Validators.required, Validators.pattern('https?://.+')])
    });
  }

  onSubmit(): void {
    if (!this.shortenUrlForm.valid) return;
    this.urlShortenerService.shortenUrl(this.shortenUrlForm.value.url);
  }
}