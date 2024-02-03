import { CommonModule } from '@angular/common';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { IxModule, ModalService, ToastService } from '@siemens/ix-angular';
import { User } from '../../../models/user';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  imports: [FormsModule, CommonModule, IxModule],
})

export class RegisterComponent {
  registerObj: User = new User();

  @ViewChild('customModal', { read: TemplateRef })
  customModalRef!: TemplateRef<any>;

  constructor(public authService: AuthService, 
    private router: Router, 
    private toastr: ToastService,
    private readonly modalService: ModalService) { }

  onSubmit(form: NgForm) {
    this.authService.loginFormSubmitted = true
    if (form.valid) {
      this.register();
    }
    else {
      this.toastr.show({
        message: 'Please enter valid form inputs!'
      });
    }
  }

  register(): void {
    this.authService.register(this.registerObj).pipe(
      catchError((error) => {
        if (error.status === 400 && error.error?.errors) {
          this.modalService.open({
            content: this.customModalRef,
            data: error.error.errors.join('\n'),
            size: '480',
          });
        }
        return throwError(error);
      })
    ).subscribe((response: any) => {
      if (response.isSuccess) {
        this.modalService.open({
          content: this.customModalRef,
          data: `User : '${response.data.username}' is successfully created.`,
          size: '480',
        });
        this.router.navigateByUrl('/login');
      }
      else {
        this.toastr.show({
          message: response.message
        });
      }
    })
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }

}
