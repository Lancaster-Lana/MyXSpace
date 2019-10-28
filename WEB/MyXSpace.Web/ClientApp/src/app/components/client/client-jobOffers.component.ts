import { Component} from '@angular/core';
import { Tenant } from 'src/app/models/tenant.model';
import { JobOffer } from 'src/app/models/jobOffer.model';
import { AuthService } from 'src/app/services/auth.service';
import { AlertService, MessageSeverity } from 'src/app/services/alert.service';
import { Utilities } from 'src/app/services/utilities';

import { ClientService } from './client.service';

@Component({
  templateUrl:'./client-jobOffers.component.html'
})
export class ClientJobOffersComponent
{
  tenant: Tenant;
  tenantID: string;
  brandName: string;

  clientID: string;
  jobOffers$: JobOffer[];
  selectedJobOffersIDs: string[] = []; //selected offers : to sign, remove, etc (change status)

  constructor(private clientService: ClientService, private authService: AuthService, private alertService: AlertService) {
    this.tenant = authService.currentTenant;
    if (this.tenant)
      this.tenantID = this.tenant.id;

    var user = authService.currentUser;
    this.clientID = user != null ? user.id : null; //TODO: find by name
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;// note: must be used 'contracts|async' - to retrieve actual data on UI

    this.clientService.getJobOffers(this.tenant, this.clientID)
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

  makeJobOffer() {
    //Get selected  
    alert("TODO: method makeJobOffer" + this.selectedJobOffersIDs);
  }
}
