import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { IxModule } from '@siemens/ix-angular';
import { AgGridModule } from 'ag-grid-angular';
import { Order } from '../../models/order';
import { Router } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { ColDef, ValueGetterParams } from 'ag-grid-community';
import { AuthService } from '../../services/auth.service';
import { User } from '../../models/user';


@Component({
  selector: 'app-order',
  standalone: true,
  templateUrl: './order.component.html',
  imports: [CommonModule, AgGridModule, IxModule],
})

export class OrderComponent implements OnInit {
  orders: Order[] = [];
  loggedUser: User = new User();
  orderCount?: number;

  colDefs: ColDef<Order>[] = [
    { field: 'name' },
    { field: 'description', width:225 },
    { field: 'totalAmount' },
  ];

  constructor(public orderService: OrderService,  private authService: AuthService, private router: Router) {
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
    this.loadOrders();
  }

  loadOrders() {
    this.orderService.getAllOrders().subscribe(
      (res) => {
        this.orders = res.data;
        this.orderCount = this.orders.length;
      },
      (error) => {
        console.error('Error loading orders:', error);
      }
    );
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
