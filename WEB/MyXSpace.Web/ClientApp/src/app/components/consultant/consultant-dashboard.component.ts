import { Component } from '@angular/core';

import { ConsultantService } from './consultant.service';
import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

import { Tenant } from 'src/app/models/tenant.model';
import { BrandNameEnum } from 'src/app/models/enums';

@Component({
  templateUrl:'./consultant-dashboard.component.html'
})

export class ConsultantDashboardComponent
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

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; 
    this.consultantID = user != null ? user.id : null; 
  }

}
