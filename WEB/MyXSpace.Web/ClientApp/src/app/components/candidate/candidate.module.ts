import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from "@angular/router";
import { GridModule } from "@progress/kendo-angular-grid";
import { CandidateDashboardComponent } from "./candidate-dashboard.component";
import { CandidateJobOffersComponent } from "./candidate-JobOffers.component";
import { CandidateContractsListComponent } from "./candidate-contracts.component";
import { CandidateDocumentsComponent } from "./candidate-documents.component";
import { CandidateService } from "./candidate.service";

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  imports: [BrowserModule, RouterModule, FormsModule, CommonModule, FormsModule, ReactiveFormsModule,
    GridModule //KENDO Grid
  ],
  declarations: [CandidateDashboardComponent , CandidateJobOffersComponent, CandidateContractsListComponent, CandidateDocumentsComponent],
  providers: [CandidateService]//, DocumentsService, CandidateAuthGuard],
  //exports: [ ]
})
export class CandidateModule { }
