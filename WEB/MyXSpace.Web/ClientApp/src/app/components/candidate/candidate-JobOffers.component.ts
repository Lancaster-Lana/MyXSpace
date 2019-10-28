import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

import { CandidateService } from './candidate.service';

import { Tenant } from '../../models/tenant.model';
import { Utilities } from '../../services/utilities';
import { JobOffer } from '../../models/jobOffer.model';

@Component({
  templateUrl:'./candidate-jobOffers.component.html'
})
export class CandidateJobOffersComponent
{
  tenant: Tenant;
  tenantID: string;
  brandName: string;

  candidateID: string;
  jobOffers$: JobOffer[];
  selectedJobOffersIDs: string[] = []; //selected offers : to sign, remove, etc (change status)

  constructor(private candidateService: CandidateService, private authService: AuthService, private alertService: AlertService) {
    this.tenant = authService.currentTenant;
    if (this.tenant)
      this.tenantID = this.tenant.id;

    var user = authService.currentUser; 
    this.candidateID = user != null ? user.id : null; //TODO: find by name
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;// note: must be used 'contracts|async' - to retrieve actual data on UI

    this.candidateService.getJobOffers(this.tenant, this.candidateID)
      .subscribe((results: Array<any>) => {

        this.alertService.stopLoadingMessage(); //this.loadingIndicator = false;
        this.jobOffers$ = results;
      },
        error => {
          this.alertService.stopLoadingMessage();
          this.alertService.showStickyMessage("Load Error", `Unable to retrieve jobOffers from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        });
  }

  acceptJobOffer() {
    //Get selected  
    alert("TODO: method acceptJobOffer" + this.selectedJobOffersIDs);
  }
}
