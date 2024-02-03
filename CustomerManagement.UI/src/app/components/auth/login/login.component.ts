import { Component, TemplateRef, ViewChild } from '@angular/core';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../services/auth.service';
import { ToastService, ModalService, IxModule } from '@siemens/ix-angular';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [FormsModule, CommonModule, IxModule],
})

export class LoginComponent {
  loginObj: any = {
    "email": "",
    "password": ""
  }
  
  @ViewChild('customModal', { read: TemplateRef })
  customModalRef!: TemplateRef<any>;

  constructor(
    public authService: AuthService,
    private cookieService: CookieService,
    private router: Router, 
    private toastr: ToastService, 
    private readonly modalService: ModalService) { }

  onSubmit(form: NgForm) {
    this.authService.loginFormSubmitted = true
    if (form.valid) {
      this.login();
    }
    else {
      this.toastr.show({
        message: 'Please enter valid form inputs!'
      });
    }
  }

  login(): void {
    this.authService.login(this.loginObj.email, this.loginObj.password).pipe(
      catchError((error) => {
        if (error.status === 401 && error.error?.errors) {
          this.modalService.open({
            content: this.customModalRef,
            data: error.error.errors.join('\n'),
            size: '480',
          });
        } 
        return throwError(error);
      })
    ).subscribe((response: any) => {
      if (response.success) {
        this.modalService.open({
          content: this.customModalRef,
          data: 'Login Successful!',
          size: '480',
        });
        this.cookieService.set('loginToken', response.token); 
        this.router.navigateByUrl('/customer');
      }
      else {
        this.toastr.show({
          message: response.message
        });
      }
    })
  }

  goToRegister(): void {
    this.router.navigate(['/register']);
  }

}
