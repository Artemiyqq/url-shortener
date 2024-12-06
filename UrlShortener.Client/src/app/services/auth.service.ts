import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { LoginDto } from '../models/login-dto.model';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) { }

  async login(loginDto: LoginDto): Promise<number> {
    return await firstValueFrom(
      this.http.post<number>(`${environment.apiUrl}/auth/login`, loginDto)
    );
  }
}
