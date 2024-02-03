import { Component } from "@angular/core";
import { IxModule } from "@siemens/ix-angular";
import { ICellRendererAngularComp } from "ag-grid-angular";
import { ICellRendererParams } from "ag-grid-community";

@Component({
  selector: 'app-view-customer-order-button-renderer',
  standalone: true,
  imports: [IxModule],
  template: `
    <ix-button class="m-1" variant="primary" icon="eye" oval (click)="btnClickHandler()">
      View
    </ix-button>
  `,
})

export class ViewCustomerOrderButtonRendererComponent implements ICellRendererAngularComp {
  refresh(params: ICellRendererParams<any, any, any>): boolean {
    throw new Error("Method not implemented.");
  }
  private params: any;

  agInit(params: any): void {
    this.params = params;
  }

  btnClickHandler() {
    this.params.clicked.onViewCustomerOrderClick(this.params.data.id);
  }
}