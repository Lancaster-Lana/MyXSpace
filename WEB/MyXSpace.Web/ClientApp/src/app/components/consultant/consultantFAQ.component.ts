import { Component } from '@angular/core';

import { ConsultantService } from './consultant.service';

import { BrandNameEnum } from 'src/app/models/enums';
import { AlertService } from 'src/app/services/alert.service';
import { AuthService } from 'src/app/services/auth.service';
import { Tenant } from 'src/app/models/tenant.model';

@Component({
  templateUrl:'./consultantFAQ.component.html'
})
export class ConsultantFAQComponent
{
  tenant: Tenant;
  brandName: string;
  tenantID: string;

  consultantID: string; // current consultant

  constructor(
    private consultantService: ConsultantService,
    private authService: AuthService,
    private alertService: AlertService)
  {
    this.tenant = authService.currentTenant; // current tenant - get from sesstion
    this.brandName = this.tenant != null ? this.tenant.name : BrandNameEnum.Default;

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.consultantID = user != null ? user.id : null; //TODO: find consultant by name
  }

  ngOnInit()
{
    this.loadData();
  }

  loadData() {
    //TODO : FAQ questions list
  }
}
