import { Component, Inject } from '@angular/core';

import { ConsultantService } from './consultant.service';
import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

import { Utilities } from 'src/app/services/utilities';
import { Contract } from 'src/app/models/contract.model';
import { Tenant } from 'src/app/models/tenant.model';

@Component({
  templateUrl:'./consultant-contracts.component.html'
})
//Client contracts
export class ConsultantContractsListComponent
{
  tenant: Tenant; // current tenant - get from sesstion
  tenantID: string;
  brandName: string;

  consultantID: string; // current consultant - get from sesstion
  contracts$: Contract[];  
  selectedContractsIDs: string[] = []; //selected contracts : to sign, remove, etc (change status)

  constructor(private consultantService: ConsultantService, private authService: AuthService, private alertService: AlertService)
  {
    this.tenant = authService.currentTenant;
    if (this.tenant)
      this.tenantID = this.tenant.id;

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.consultantID = user != null ? user.id : null; //TODO: find consultant by name
  }

  ngOnInit() {
    this.loadData();
  }

  loadData()
  {
    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;// note: must be used 'contracts|async' - to retrieve actual data on UI

    this.consultantService.getContracts(this.tenant, this.consultantID)
      .subscribe((results: Array<any>) => {

        this.alertService.stopLoadingMessage(); //this.loadingIndicator = false;
        this.contracts$ = results;
        //let permissions = results[1];
        //this.rowsCache = [...contracts$]; this.rows = contracts$;
      },
        error => {
          this.alertService.stopLoadingMessage();//this.loadingIndicator = false;
          this.alertService.showStickyMessage("Load Error", `Unable to retrieve contracts from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        });
  }

  signContracts()
  {
    //TODO: selected contracts
    this.selectedContractsIDs.forEach((contract) => { this.signContract(contract); }); 
  }

  signContract(contract) {
    //Get selected contract 
    alert("TODO: method signContract" + this.selectedContractsIDs);
  }
}
