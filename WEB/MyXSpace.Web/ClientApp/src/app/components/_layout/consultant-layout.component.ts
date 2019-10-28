import { Component, OnInit, Inject, Input, AfterViewInit, AfterContentInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { Tenant } from 'src/app/models/tenant.model';
import { BrandNameEnum } from 'src/app/models/enums';
import { ConsultantService } from '../consultant/consultant.service';
//import { UserProfile } from 'src/app/models/user-profie.model';

@Component({
  selector: 'consultant-layout',
  templateUrl: './consultant-layout.component.html',
  styleUrls: []
})
export class ConsultantLayoutComponent implements OnInit, AfterViewInit, AfterContentInit {

  currentMenu: string; // selected\active menu item

  brandName: string;
  tenant: Tenant; // current tenant - get from sesstion
  user: User; // current consultant user
  consultantID: string;

  //userProfile: UserProfile;
  photo: string = "../../assets/images/profile-photo-default.png"; //require("../../assets/images/profile-photo-default.png");

  constructor(
    private authService: AuthService,
    private consultantService: ConsultantService) {

    this.tenant = authService.currentTenant;
    this.brandName = this.tenant != null ? this.tenant.name : BrandNameEnum.Default;

    this.user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    if (this.user) {
      this.consultantID = this.user.id; //TODO: Q is consultantId= user.ID ?
      //this.userProfile = userProfileService.getUserProfile(this.user.id) ?? new UserProfile();
      // if (!this.userProfile.photo) this.userProfile.photo = require("../../assets/images/profile-photo-default.png");
    }
  }

  ngOnInit() { }
  ngAfterViewInit() { }
  ngAfterContentInit() { }

  changeMenu(selectedPage: any) {

    this.currentMenu = selectedPage.name;
  }

  public get candidates() {
    return this.consultantService.getCandidates(this.tenant, this.consultantID);
  }

  public get clients() {
    return this.consultantService.getClients(this.tenant, this.consultantID);
  }

  public get contracts() {
    return this.consultantService.getContracts(this.tenant, this.consultantID);
  }
}
