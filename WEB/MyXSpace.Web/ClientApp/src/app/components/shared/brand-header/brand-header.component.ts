import { Component, Input } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AlertService } from 'src/app/services/alert.service';
import { AppTranslationService } from 'src/app/services/app-translation.service';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { AccountService } from 'src/app/services/account.service';
import { BrandNameEnum } from 'src/app/models/enums';

@Component({
  selector: 'brand-header',
  templateUrl: './brand-header.component.html',
  styleUrls: ['./brand-header.component.css']
})
export class BrandHeaderComponent
{
  @Input()
  brandName: string = BrandNameEnum.Default; //default brand -passed from owner page\session. Default Brand 

  appBrandTitle: string;
  appBrandLogo = require("../../../assets/images/logo-default.png");
  appBrandHeaderBg: string;

  get isUserLoggedIn() : boolean {
    return this.authService.isLoggedIn;
  }

  get userName() {
    return !this.isUserLoggedIn ? "" : this.authService.currentUser.email + " | " + this.authService.currentUser.roles[0];/*this.authService.currentUser.friendlyName ??*/
  }

  logout() {
    this.authService.logout();
    this.authService.redirectLogoutUser();
  }

  constructor(
    public configurations: ConfigurationService, private authService: AuthService,private accountService: AccountService,
    private alertService: AlertService,  
    private translationService: AppTranslationService)
  {
    if (authService.currentTenant) {
      //replace with current brand (from session)
      this.appBrandTitle = this.brandName = authService.currentTenant.name;  //NOTE: brandName came from 'parent' control
      if (this.brandName) {
        this.appBrandLogo = require(`../../../assets/images/logo-${this.brandName}.png`);
        this.appBrandHeaderBg = `bg-header-${this.brandName}`;
      }
    }
  }

  ngOnInit() {}

  ngOnDestroy() {
    //this.languageChangedSubscription.unsubscribe();
  }
}
