import { CommonModule } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { IxModule, ModalService, ToastService } from '@siemens/ix-angular';
import { AgGridModule } from 'ag-grid-angular';
import { Order } from '../../../models/order';
import { ColDef } from 'ag-grid-community';
import { OrderService } from '../../../services/order.service';
import { CustomerService } from '../../../services/customer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../../models/user';
import { AuthService } from '../../../services/auth.service';
import { Customer } from '../../../models/customer';
import { CustomerRendererComponent } from '../../custom/customer-renderer/customer-renderer.component';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-customer-orders',
  standalone: true,
  imports: [CommonModule, AgGridModule, IxModule],
  templateUrl: './customer-orders.component.html',
})

export class CustomerOrdersComponent implements OnInit {
  orders: Order[] = [];
  loggedUser: User = new User();
  customerId: string = '';
  customer: Customer = new Customer();
  orderCount?: number;

  @ViewChild('customModal', { read: TemplateRef })
  customModalRef!: TemplateRef<any>;

  colDefs: ColDef<Order>[] = [
    { field: 'name' },
    { field: 'description' },
    { field: 'totalAmount' },
    {
      headerName: 'Customer',
      cellRenderer: CustomerRendererComponent
    },
  ];

  constructor(
    public orderService: OrderService, 
    private authService: AuthService, 
    private customerService: CustomerService, 
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastService,
    private readonly modalService: ModalService) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.customerId = params['id'];
      this.authService.getUser().subscribe(
        (user) => {
          this.loggedUser = user.data;
        },
        (error) => {
          console.error('Error fetching user:', error);
        }
      );      
      this.getCustomer();
    });
  }

  getCustomer() {
    this.customerService.getCustomerById(this.customerId).subscribe(
      (res) => {
        this.customer = res.data;
        this.loadCustomerOrders();
      },
      (error) => {
        console.error('Error fetching customer:', error);
      }
    );    
  }

  loadCustomerOrders(): void {
    this.orderService.getOrdersByCustomerId(this.customerId).pipe(
      catchError((error) => {
        if (error.status === 404) {
          debugger;
          this.modalService.open({
            content: this.customModalRef,
            data: error.error.errors,
            size: '600',
          });
          this.router.navigate(['/customer']);
        }
        return throwError(error);
      })
    ).subscribe((response: any) => {
      if (response.isSuccess) {
        this.orders = response.data.map((order: Order) => ({
          ...order,
          customer: this.customer,
        }));
        this.orderCount = this.orders.length;
      }
      else {
        this.toastr.show({
          message: 'Error loading customer orders:'
        });
      }
    })
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