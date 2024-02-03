import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'app-customer-renderer',
  standalone: true,
  template: `
    {{ customerData.firstName }} {{ customerData.lastName }}
  `,
})

export class CustomerRendererComponent implements ICellRendererAngularComp {
  private params!: ICellRendererParams<any>;

  agInit(params: ICellRendererParams<any>): void {
    this.params = params;
  }

  refresh(params: ICellRendererParams<any>): boolean {
    this.params = params;
    return true;
  }

  get customerData(): any {
    return this.params.data.customer;
  }
}
