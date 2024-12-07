import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { firstValueFrom } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UrlShortenerService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  async shortenUrl(longUrl: string) {
    const token = this.authService.getToken();
    const httpHeaders: HttpHeaders = new HttpHeaders({
      Authorization: `Bearer ${token}`});
    await firstValueFrom(this.http.get<void>(`${environment.apiUrl}/url-shortener?longUrl=${longUrl}`,
                                              {headers: httpHeaders} ));
  }
}