import { Component, OnInit, ChangeDetectorRef} from "@angular/core";
import { AuthService } from "src/app/services/auth.service";
import { Tenant } from "src/app/models/tenant.model";
import { User } from "src/app/models/user.model";
import { AccountService } from "src/app/services/account.service";

//import { UsersService } from "../../services/user.service";
//import { RoleService } from "../../services/role.service";
//import { ClientService } from "../../services/client.service";
//import { CandidateService } from "../../services/candidate.service";

//include jQuery plugins support
//var $ = require('jquery');

@Component({
  templateUrl: "admin-layout.component.html"
})
export class AdminLayoutComponent implements OnInit
{
  tenant: Tenant; // current tenant - get from session
  brandName: string;

  user: User; // current admin user
  currentMenu: string; // selected\active menu item 

  constructor(// private chRef: ChangeDetectorRef
    private authService: AuthService,
    private accountService: AccountService
   )
  {
    this.tenant = authService.currentTenant;

    if (this.tenant)
      this.brandName = this.tenant.name;

    this.user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    if (this.user) {
   
    }
  }

  ngOnInit()
  {

  }

  //get userName() {
  //  return this.authService.name;
  //}

  get roles() {
    return this.accountService.getRoles();
  }

  get users() {
    return this.accountService.getUsers();
  }
}
