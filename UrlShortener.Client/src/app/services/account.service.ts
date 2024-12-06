import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private isAdmin: boolean = false;

  constructor() { }
}
