import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./components/auth/login/login.component";
import { RegisterComponent } from "./components/auth/register/register.component";
import { CustomerComponent } from "./components/customer/customer.component";
import { OrderComponent } from './components/order/order.component';
import { AddCustomerComponent } from "./components/customer/add-customer/add-customer.component";
import { EditCustomerComponent } from "./components/customer/edit-customer/edit-customer.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet, 
    LoginComponent, 
    RegisterComponent, 
    CustomerComponent, 
    OrderComponent, 
    AddCustomerComponent, 
    EditCustomerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'CustomerManagement.UI';
}
