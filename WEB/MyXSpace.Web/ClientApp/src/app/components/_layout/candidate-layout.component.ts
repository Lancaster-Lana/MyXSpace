import { Component, OnInit, Inject } from '@angular/core';
import { Tenant } from 'src/app/models/tenant.model';
import { User } from 'src/app/models/user.model';
//import { UserProfile } from 'src/app/models/user-profie.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'candidate-layout',
  templateUrl: './candidate-layout.component.html',
  styleUrls: [  ]
})
export class CandidateLayoutComponent implements OnInit 
{

  currentMenu: string; // selected\active menu item

  tenant: Tenant; // current tenant - get from session
  brandName: string;

  user: User; // current candidate user
  //userProfile: UserProfile;
  candidateId: string;

  photo: string = "../../assets/images/profile-photo-default.png"; //require("../../assets/images/profile-photo-default.png");

  constructor(private authService: AuthService)
  {
    this.tenant = authService.currentTenant;
    if (this.tenant)
      this.brandName = this.tenant.name;

    this.user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; <-id from route or session
    if (this.user) {

      this.candidateId = this.user.id; //TODO: Q: is consultantId= user.ID ?

      //this.userProfile = userProfileService.getUserProfile(this.user.id) ??  new UserProfile();
      //if (!this.userProfile.photo) this.userProfile.photo = require("../../assets/images/profile-photo-default.png");
    }
  }

  ngOnInit() {

  }

  changeMenu(selectedPage: any) {
    this.currentMenu = selectedPage.name;
  }
}
