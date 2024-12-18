import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { firstValueFrom } from 'rxjs';
import { AuthService } from './auth.service';
import { ShortenedUrlDto } from '../models/shortened-url-dto.model';
import { LongUrlDto } from '../models/long-url-dto.model';

@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {
  constructor(private http: HttpClient, private authService: AuthService) { }

  async getLongUrl(shortUrl: string) {
    const longUrlDto = await firstValueFrom(
      this.http.get<LongUrlDto>(`${environment.apiUrl}/url-shortener/long-url/${shortUrl}`)
    );
    return longUrlDto.longUrl;
  }

  async getAll(): Promise<ShortenedUrlDto[] | undefined> {
    const responce = await firstValueFrom(
      this.http.get<ShortenedUrlDto[]>(`${environment.apiUrl}/url-shortener`)
    );
    for (let i = 0; i < responce.length; i++) {
      responce[i].shortUrl = `${environment.clientUrl}/${responce[i].shortUrl}`;
    }
    return responce;
  }

  async shortenUrl(longUrl: string): Promise<ShortenedUrlDto> {
    const token = this.authService.getToken();
    const httpHeaders: HttpHeaders = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    const url = `${environment.apiUrl}/url-shortener/shorten?longUrl=${longUrl}`
    const longUrlDto = new LongUrlDto(longUrl);

    const responce = await firstValueFrom(this.http.post<ShortenedUrlDto>(url, longUrlDto, { headers: httpHeaders }));

    console.log(responce);
    
    responce.shortUrl = `${environment.clientUrl}/${responce.shortUrl}`;

    return responce;
  }

  async deleteByIndex(index: number): Promise<void> {
    const token = this.authService.getToken();
    const httpHeaders: HttpHeaders = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    await firstValueFrom(
      this.http.delete(`${environment.apiUrl}/url-shortener/${index}`, { headers: httpHeaders })
    );
  }
}