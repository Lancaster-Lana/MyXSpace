import { Component, OnInit, Inject } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Tenant } from 'src/app/models/tenant.model';
import { User } from 'src/app/models/user.model';
import { BrandNameEnum } from 'src/app/models/enums';

@Component({
  selector: 'client-layout',
  templateUrl: './client-layout.component.html',
  styleUrls: [
        //'../Content/vendor/bootstrap/css/bootstrap.min.css',
    ]
})
export class ClientLayoutComponent implements OnInit 
{
  tenant: Tenant; // current tenant - get from sesstion
  brandName: string;

  user: User; // current client user
  clientId: string;// TODO: Q clientId= user.ID ?

  currentMenu: string; // selected\active menu item

  photo: string = "../../assets/images/profile-photo-default.png"; //require("../../assets/images/profile-photo-default.png");

  constructor(private authService: AuthService)
  {
    this.tenant = authService.currentTenant;

    if (this.tenant)
      this.brandName = this.tenant.name;

    this.user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    if (this.user) {
      this.clientId = this.user.id; //TODO: Q clientId= user.ID ?
    }
  }

  ngOnInit() {

  }

  changeMenu(selectedPage: any) {
    this.currentMenu = selectedPage.name;
  }
}
