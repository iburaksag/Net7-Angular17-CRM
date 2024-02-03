import { CommonModule } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IxModule, ModalService, ToastService } from '@siemens/ix-angular';
import { Customer } from '../../../models/customer';
import { CustomerService } from '../../../services/customer.service';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-edit-customer',
  standalone: true,
  templateUrl: './edit-customer.component.html',
  imports: [FormsModule, CommonModule, IxModule]
})

export class EditCustomerComponent implements OnInit {
  customerObj: Customer = new Customer();
  customerId: string = '';
  
  @ViewChild('customModal', { read: TemplateRef })
  customModalRef!: TemplateRef<any>;

  constructor(
    public customerService: CustomerService, 
    private router: Router, 
    private route: ActivatedRoute,
    private toastr: ToastService, 
    private readonly modalService: ModalService) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.customerId = params['id'];
      this.loadCustomerData();
    });
  }

  loadCustomerData(): void {
    this.customerService.getCustomerById(this.customerId).subscribe(
      (res) => {
        this.customerObj = res.data;
      },
      (error) => {
        console.error('Error loading customer details:', error);
      }
    );
  }

  onUpdateClick(form: NgForm): void {
    debugger;
    this.customerService.putCustomer(this.customerObj).pipe(
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
          data: ['Customer is updated successfully!'],
          size: '480',
        });
        this.router.navigateByUrl('/customer');
      } else {
        this.toastr.show({
          message: response.message
        });
      }
    });
  }

  onDeleteClick(): void {
    const confirmDelete = confirm('Are you sure you want to delete this customer?');

    if (confirmDelete) {
      this.customerService.deleteCustomer(this.customerObj.id).subscribe(
        () => {
          this.modalService.open({
            content: this.customModalRef,
            data: 'Customer is successfully deleted.',
            size: '480',
          });
          this.router.navigateByUrl('/customer');
        },
        (error) => {
          console.error('Error deleting customer:', error);
          this.toastr.show({
            message: 'Unknown error occurred'
          });
        }
      );
    }
  }

  backButtonClick(): void {
    this.router.navigate(['/customer']);
  }

}
