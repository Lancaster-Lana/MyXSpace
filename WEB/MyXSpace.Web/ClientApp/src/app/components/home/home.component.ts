
import { Component, Inject } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { ConfigurationService } from '../../services/configuration.service';
import { Tenant } from 'src/app/models/tenant.model';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { BrandNameEnum } from 'src/app/models/enums';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    animations: [fadeInOut]
})
export class HomeComponent
{
  public tenant: Tenant; 
  public brandName: string;

  constructor(@Inject('BASE_URL') baseUrl: string,
    private router: Router, activeRoute: ActivatedRoute,
    private authService: AuthService)
  {
    this.tenant = authService.currentTenant;// current tenant - get from sesstion

    this.brandName = this.tenant != null ? this.tenant.name : BrandNameEnum.Default;

  }
}
