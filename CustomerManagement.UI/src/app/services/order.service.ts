import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  url: string = environment.apiBaseUrl + '/order';

  constructor(private http: HttpClient) { }

  getAllOrders(): Observable<any> {
    return this.http.get<any[]>(this.url);
  }

  getOrdersByCustomerId(customerId: string): Observable<any> {
    return this.http.get<any>(`${this.url}/customer/${customerId}`);
  }
}
