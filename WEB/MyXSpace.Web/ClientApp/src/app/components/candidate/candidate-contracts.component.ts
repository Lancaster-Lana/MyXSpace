import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

import { CandidateService } from './candidate.service';

import { Utilities } from '../../services/utilities';
import { Contract } from '../../models/contract.model';
import { Tenant } from '../../models/tenant.model';

@Component({
  templateUrl:'./candidate-contracts.component.html'
})
//Candidate contracts
export class CandidateContractsListComponent
{
  tenant: Tenant; // current tenant - get from sesstion
  brandName: string; //brandName = tenant.name

  candidateID: string;  
  contracts$: Contract[];  
  selectedContractsIDs: string[] = []; //selected contracts : to sign, remove, etc (change status)

  constructor(private candidateService: CandidateService, private authService: AuthService, private alertService: AlertService)
  {
    this.tenant = authService.currentTenant;
    if (this.tenant)
      this.brandName = this.tenant.name;

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.candidateID = user != null ? user.id : null; //TODO: find canddate by name
  }

  ngOnInit()
  {
    this.loadData();
  }

  loadData()
  {
    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;// note: must be used 'contracts|async' - to retrieve actual data on UI

    this.candidateService.getContracts(this.tenant, this.candidateID)
      .subscribe((results: Array<any>) => {

        this.alertService.stopLoadingMessage(); //this.loadingIndicator = false;
        this.contracts$ = results;
      },
        error => {
          this.alertService.stopLoadingMessage();//this.loadingIndicator = false;
          this.alertService.showStickyMessage("Load Error", `Unable to retrieve contracts from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        });
  }

  signContracts() {
    //TODO: selected contracts
    this.selectedContractsIDs.forEach((contract) => { this.signContract(contract); });
  }

  signContract(contract) {
    //Get selected contract 
    alert("TODO: method signContract" + this.selectedContractsIDs);
  }
}
