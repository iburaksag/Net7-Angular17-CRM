import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AgGridModule } from 'ag-grid-angular';
import { ColDef } from 'ag-grid-community'; 
import { CustomerService } from '../../services/customer.service';
import { Router } from '@angular/router';
import { Customer } from '../../models/customer';
import { IxModule } from '@siemens/ix-angular';
import { User } from '../../models/user';
import { AuthService } from '../../services/auth.service';
import { EditButtonRendererComponent } from '../custom/edit-button-renderer/edit-button-renderer.component';
import { ViewCustomerOrderButtonRendererComponent } from '../custom/view-customer-order-button-renderer/view-customer-order-button-renderer.component';

@Component({
  selector: 'app-customer',
  standalone: true,
  templateUrl: './customer.component.html',
  imports: [CommonModule, AgGridModule, IxModule],
})

export class CustomerComponent implements OnInit {
  customers: Customer[] = [];
  loggedUser: User = new User();
  customerCount?: number;

  params: any;

  colDefs: ColDef<Customer>[] = [
    { field: 'firstName' },
    { field: 'lastName' },
    { field: 'email', width: 165 },
    { field: 'phone' },
    { field: 'city' },
    { field: 'country' },
    {
      headerName: 'Orders',
      cellRenderer: ViewCustomerOrderButtonRendererComponent,
      cellRendererParams: {
        clicked: { onViewCustomerOrderClick: this.onViewCustomerOrderClick.bind(this) },
      },
    },
    {
      headerName: '',
      cellRenderer: EditButtonRendererComponent,
      cellRendererParams: {
        clicked: { onEditClick: this.onEditClick.bind(this) },
      },
    }
  ];

  constructor(public customerService: CustomerService, private authService: AuthService, private router: Router){
  }

  ngOnInit(): void {
    this.authService.getUser().subscribe(
      (user) => {
        this.loggedUser = user.data;
      },
      (error) => {
        console.error('Error fetching user:', error);
      }
    );
    this.loadCustomers();
  }

  loadCustomers() {
    this.customerService.getAllCustomers().subscribe(
      (res) => {
        this.customers = res.data;
        this.customerCount = this.customers.length;
      },
      (error) => {
        console.error('Error loading customers:', error);
      }
    );
  }

  onViewCustomerOrderClick(customerId: string): void {
    this.router.navigate(['/customer-orders', customerId]);
  }

  onEditClick(customerId: string): void {
    this.router.navigate(['/edit-customer', customerId]);
  }
  
  navigateToAddCustomer(): void {
    this.router.navigate(['/add-customer']);
  }

  logoutClick(): void {
    this.authService.logout();
  }

  goToCustomers(): void {
    this.router.navigate(['/customer']);
  }
  goToOrders(): void {
    this.router.navigate(['/order']);
  }
}
