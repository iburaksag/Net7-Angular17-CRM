import { CommonModule } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Customer } from '../../../models/customer';
import { IxModule, ModalService, ToastService } from '@siemens/ix-angular';
import { CustomerService } from '../../../services/customer.service';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-add-customer',
  standalone: true,
  templateUrl: './add-customer.component.html',
  imports: [FormsModule, CommonModule, IxModule]
})

export class AddCustomerComponent{
  customerObj: Customer = new Customer();

  @ViewChild('customModal', { read: TemplateRef })
  customModalRef!: TemplateRef<any>;

  constructor(
    public customerService: CustomerService,
    private router: Router, 
    private toastr: ToastService, 
    private readonly modalService: ModalService) { }


  onSubmit(form: NgForm) {
    if (form.valid) {
      this.createCustomer();
    }
    else {
      this.toastr.show({
        message: 'Please enter valid form inputs!'
      });
    }
  }

  createCustomer(): void {
    this.customerService.postCustomer(this.customerObj).pipe(
      catchError((error) => {
        if (error.status === 400 && error.error?.errors) {
          this.modalService.open({
            content: this.customModalRef,
            data: error.error.errors,
            size: '600',
          });
        }
        return throwError(error);
      })
    ).subscribe((response: any) => {
      if (response.isSuccess) {
        this.modalService.open({
          content: this.customModalRef,
          data: ['Customer is created successfully!'],
          size: '480',
        });
        this.router.navigateByUrl('/customer');
      }
      else {
        this.toastr.show({
          message: response.message
        });
      }
    })
  }

  backButtonClick(): void {
    this.router.navigate(['/customer']);
  }
}
