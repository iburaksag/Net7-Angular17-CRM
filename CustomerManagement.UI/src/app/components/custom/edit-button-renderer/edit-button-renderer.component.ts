import { Component } from "@angular/core";
import { IxModule } from "@siemens/ix-angular";
import { ICellRendererAngularComp } from "ag-grid-angular";
import { ICellRendererParams } from "ag-grid-community";

@Component({
  selector: 'app-edit-button-renderer',
  standalone:true,
  imports:[IxModule],
  template: `
    <ix-button class="m-1" variant="secondary" icon="pen-filled" oval outline (click)="btnClickedHandler()">
      
    </ix-button>
  `,
})
export class EditButtonRendererComponent implements ICellRendererAngularComp {
  refresh(params: ICellRendererParams<any, any, any>): boolean {
    throw new Error("Method not implemented.");
  }
  private params: any;

  agInit(params: any): void {
    this.params = params;
  }

  btnClickedHandler() {
    this.params.clicked.onEditClick(this.params.data.id);
  }
}
