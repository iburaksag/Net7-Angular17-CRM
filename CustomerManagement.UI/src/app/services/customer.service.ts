import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  url: string = environment.apiBaseUrl + '/customer';

  constructor(private http: HttpClient) { }

  getAllCustomers(): Observable<any> {
    return this.http.get<any[]>(this.url);
  }

  getCustomerById(customerId: string): Observable<any> {
    return this.http.get<any>(`${this.url}/${customerId}`);
  }

  postCustomer(customer: Customer): Observable<any> {
    return this.http.post<any>(`${this.url}`, customer);
  }

  putCustomer(customer: Customer): Observable<any> {
    debugger;
    return this.http.put<any>(`${this.url}/${customer.id}`, customer);
  }

  deleteCustomer(customerId:any) {
    return this.http.delete(this.url + '/' + customerId);    
  }
}
