import { Component, Inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

import { ClientService } from './client.service';

import { Utilities } from '../../services/utilities';
import { Contract } from '../../models/contract.model';
import { Tenant } from '../../models/tenant.model';

@Component({
  templateUrl:'./client-contracts.component.html'
})
//Candidate contracts
export class ClientContractsListComponent
{
  tenant: Tenant; // current tenant - get from sesstion
  brandName: string; //brandName = tenant.name

  clientID: string;  
  contracts$: Contract[];  
  selectedContractsIDs: string[] = []; //selected contracts : to sign, remove, etc (change status)

  constructor(private clientService: ClientService, private authService: AuthService, private alertService: AlertService)
  {
    this.tenant = authService.currentTenant;
    if (this.tenant)
      this.brandName = this.tenant.name;

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.clientID = user != null ? user.id : null; //TODO: find canddate by name
  }

  ngOnInit() {
    this.loadData();
  }

  loadData()
  {
    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;// note: must be used 'contracts|async' - to retrieve actual data on UI

    this.clientService.getContracts(this.tenant, this.clientID)
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
