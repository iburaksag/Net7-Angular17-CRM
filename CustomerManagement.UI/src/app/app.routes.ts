import { Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component'
import { RegisterComponent } from './components/auth/register/register.component';
import { CustomerComponent } from './components/customer/customer.component';
import { OrderComponent } from './components/order/order.component';
import { AddCustomerComponent } from './components/customer/add-customer/add-customer.component';
import { EditCustomerComponent } from './components/customer/edit-customer/edit-customer.component';
import { CustomerOrdersComponent } from './components/order/customer-orders/customer-orders.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'customer', component: CustomerComponent, canActivate: [authGuard] },
    { path: 'order', component: OrderComponent, canActivate: [authGuard] },    
    { path: 'add-customer', component: AddCustomerComponent, canActivate: [authGuard] },
    { path: 'edit-customer/:id', component: EditCustomerComponent, canActivate: [authGuard] },
    { path: 'customer-orders/:id', component: CustomerOrdersComponent, canActivate: [authGuard] },
    { path: '', redirectTo: 'login', pathMatch:'full' },
    { path: '**', component: LoginComponent },
];
