import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { JwtPayload, jwtDecode } from "jwt-decode";
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  url: string = environment.apiBaseUrl + '/user';
  loginFormSubmitted: boolean = false;

  constructor(private http: HttpClient,private cookieService: CookieService, private router: Router) { }

  isAuthenticated(): boolean {
    const token = this.cookieService.get('loginToken');
    return !!token; // Returns true if the token is present
  }

  getUserIdFromToken(): JwtPayload | null {
    const token = this.cookieService.get('loginToken');  
    if (token) {
      try {
        const decoded = jwtDecode(token);
        return decoded;
      } catch (error) {
        console.error('Error decoding token:', error);
      }
    }
    return null;
  }

  getUser(): Observable<any> {
    const userId = this.getUserIdFromToken()?.sub;
    return this.http.get<any>(`${this.url}/${userId}`);
  }

  public register(user: User): Observable<any> {
    return this.http.post<any>(`${this.url}/register`, user); 
  }

  public login(email: string, password: string): Observable<any> {
    const userCredentials = { email, password };
    return this.http.post(`${this.url}/login`, userCredentials);
  }

  public logout(): void {
    this.cookieService.delete('loginToken');
    this.router.navigate(['/login']);
  }

}


