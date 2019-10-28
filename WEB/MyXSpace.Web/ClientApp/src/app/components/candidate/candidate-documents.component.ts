import { Component } from '@angular/core';

import { CandidateService } from './candidate.service';

import { BrandNameEnum } from 'src/app/models/enums';
import { AlertService, MessageSeverity } from 'src/app/services/alert.service';
import { AuthService } from 'src/app/services/auth.service';
import { Tenant } from 'src/app/models/tenant.model';
import { Utilities } from 'src/app/services/utilities';

@Component({
  templateUrl:'./candidate-documents.component.html'
})
export class CandidateDocumentsComponent
{
  tenant: Tenant;
  brandName: string;
  tenantID: string;

  candidateID: string;

  documents$: Document[];
  selectedDocumentIDs: string[] = []; //selected Documents : to open\save pdf, remove, etc

  constructor(
    private candidateService: CandidateService,
    private authService: AuthService,
    private alertService: AlertService)
  {
    this.tenant = authService.currentTenant; // current tenant - get from sesstion
    this.brandName = this.tenant != null ? this.tenant.name : BrandNameEnum.Default;

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.candidateID = user != null ? user.id : null; //TODO: find by name
  }

  ngOnInit() {

    this.loadData(this.candidateID);
  }

  loadData(candidateId: string) {
    //TODO:
    this.candidateService.getDocuments(this.tenant, this.candidateID)
      .subscribe((results: Array<any>) => {

      this.alertService.stopLoadingMessage(); //this.loadingIndicator = false;
        this.documents$ = results;
    },
      error => {
        this.alertService.stopLoadingMessage();//this.loadingIndicator = false;
        this.alertService.showStickyMessage("Load Error", `Unable to retrieve candidate documents from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
          MessageSeverity.error, error);
      });
  }
}
