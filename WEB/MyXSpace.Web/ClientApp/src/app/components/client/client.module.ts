import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from "@angular/router";
import { GridModule } from "@progress/kendo-angular-grid";

import { ClientService } from "./client.service";
import { ClientDashboardComponent } from "./client-dashboard.component";
import { ClientJobOffersComponent } from "./client-jobOffers.component";
import { ClientContractsListComponent } from "./client-contracts.component";

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  imports: [BrowserModule, RouterModule, FormsModule, CommonModule, FormsModule, ReactiveFormsModule,
    GridModule //KENDO Grid
  ],
  declarations: [ClientDashboardComponent, ClientJobOffersComponent, ClientContractsListComponent], //ClientDocumentsComponent, ],
  providers: [ClientService], //DocumentsService],
  //exports: [ ClientDashboardComponent  ]
})
export class ClientModule { }
