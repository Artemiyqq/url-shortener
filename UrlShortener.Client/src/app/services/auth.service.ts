import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { LoginDto } from '../models/login-dto.model';
import { firstValueFrom } from 'rxjs';
import { RegisterDto } from '../models/register-dto.model';
import { TokenDto } from '../models/token-dto.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private tokenKey = 'user-token';
  private isAuthenticated: boolean = false;

  constructor(private http: HttpClient) {}

  private checkToken(): boolean {
    const token = this.getToken();

    if (typeof token === 'string' && token) {
      try {
        const tokenDto = new TokenDto(token);
        firstValueFrom(
          this.http.post(`${environment.apiUrl}/auth/check-token`, tokenDto)
        );
        return true;
      } catch (error) {
        localStorage.removeItem(this.tokenKey);
        return false;
      }
    } else {
      return false;
    }
  }

  async login(loginDto: LoginDto): Promise<void> {
    const response = await firstValueFrom(
      this.http.post<any>(`${environment.apiUrl}/auth/login`, loginDto)
    );
    localStorage.setItem(this.tokenKey, response.token);
    this.isAuthenticated = true;
  }

  async register(registerDto: RegisterDto): Promise<void> {
    await firstValueFrom(
      this.http.post<void>(`${environment.apiUrl}/auth/register`, registerDto)
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getIsAuthenticated(): boolean {
    const result = this.checkToken();
    this.isAuthenticated = result;
    return this.isAuthenticated;
  }
}