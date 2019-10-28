import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from "@angular/router";
import { GridModule } from "@progress/kendo-angular-grid";

import { ConsultantService } from "./consultant.service";

import { ConsultantDashboardComponent } from "./consultant-dashboard.component";
import { ConsultantCandidatesListComponent } from "./consultant-candidates.component";
import { ConsultantClientsListComponent } from "./consultant-clients.component";
import { ConsultantContractsListComponent } from "./consultant-contracts.component";
import { ConsultantReportingComponent } from "./consultant-reporting.component";
import { ConsultantFAQComponent } from "./consultantFAQ.component";

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  imports: [BrowserModule, RouterModule, FormsModule, CommonModule, FormsModule, ReactiveFormsModule,
    GridModule //KENDO Grid
  ],
  declarations: [ConsultantDashboardComponent, ConsultantCandidatesListComponent, ConsultantClientsListComponent, ConsultantContractsListComponent, ConsultantReportingComponent, ConsultantFAQComponent],
  providers: [ConsultantService],//DocumentsService],
  exports: [ ]
})
export class ConsultantModule { }
