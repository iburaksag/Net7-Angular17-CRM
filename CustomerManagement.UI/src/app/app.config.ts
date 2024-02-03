import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { AgGridModule } from 'ag-grid-angular';
import { BrowserModule } from '@angular/platform-browser';
import { IxModule } from '@siemens/ix-angular';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { HttpClientModule, provideHttpClient, withFetch, withInterceptors } from "@angular/common/http";
import { FormsModule } from '@angular/forms';
import { authInterceptor } from './services/interceptor/auth.interceptor';
import { EditButtonRendererComponent } from './components/custom/edit-button-renderer/edit-button-renderer.component';
import { ViewCustomerOrderButtonRendererComponent } from './components/custom/view-customer-order-button-renderer/view-customer-order-button-renderer.component';
import { CustomerRendererComponent } from './components/custom/customer-renderer/customer-renderer.component';
import { CookieService } from 'ngx-cookie-service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideClientHydration(),
    provideHttpClient(withFetch(), withInterceptors([authInterceptor])),
    EditButtonRendererComponent,
    ViewCustomerOrderButtonRendererComponent,
    CustomerRendererComponent,
    importProvidersFrom(
      HttpClientModule, 
      FormsModule,
      BrowserModule,
      AgGridModule,
      CookieService,
      IxModule.forRoot(),
      BrowserAnimationsModule)
  ],
};
