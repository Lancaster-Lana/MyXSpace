import { Component, OnInit, OnDestroy, Input, AfterViewInit, AfterContentInit } from "@angular/core";
import { Router } from "@angular/router";

import { AlertService, MessageSeverity, DialogType } from '../../services/alert.service';
import { AuthService } from "../../services/auth.service";
import { ConfigurationService } from '../../services/configuration.service';
import { Utilities } from '../../services/utilities';
import { UserLogin } from '../../models/user-login.model';
import { AccountService } from "src/app/services/account.service";
import { Tenant } from "src/app/models/tenant.model";
import { Observable } from "rxjs";

@Component({
  selector: "app-login",
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'
  ]
})
export class LoginComponent implements OnInit, OnDestroy {

  //Current brand\tenant
  currentTenant: Tenant;
  currentTenantObser: Observable<Tenant>;

  userLogin = new UserLogin();
  isLoading = false;
  formResetToggle = true;
  modalClosedCallback: () => void;
  loginStatusSubscription: any;

  @Input()
  isModal = false;

  private redirectToRoleDefaultPage(role) {
    switch (role) {
      case 'consultant':
        this.router.navigate(['/consultant']);
        break;
      case 'candidate':
        this.router.navigate(['/candidate']);
        break;
      case 'client':
        this.router.navigate(['/client']);
        break;
      case 'administrator': //NOTE tenant admin
        this.router.navigate(['/admin']);
        break;

      default:
        this.router.navigate(['/home']); //NOTE: some common information

        break;
    }
  }

  constructor(
    private router: Router,
    private alertService: AlertService,
    private authService: AuthService, private accountService: AccountService,
    private configurations: ConfigurationService) {
    if (this.authService.currentTenant)
      this.currentTenant = this.authService.currentTenant;
    this.currentTenantObser = this.accountService.currentTenant;
  }

  ngOnInit() {

    this.userLogin.rememberMe = this.authService.rememberMe;

    /*
      if (this.getShouldRedirect()) {
        this.authService.redirectLoginUser();
      }
      else {
        this.loginStatusSubscription = this.authService.getLoginStatusEvent()
          .subscribe(isLoggedIn => {
   
            if (this.getShouldRedirect()) {
              this.authService.redirectLoginUser();
            }
            else {
              alert('no redirect');
              this.router.navigate(['/home']); //home
            }
        });
      }*/
  }

  login() {
    this.isLoading = true;
    this.alertService.startLoadingMessage("", "Attempting login...");

    //Try login to tenant\brand under credentials
    this.authService.login(this.userLogin.email, this.userLogin.password, this.userLogin.rememberMe)
      .subscribe(user => {
        setTimeout(() => {
          this.alertService.stopLoadingMessage();
          this.isLoading = false;
          this.reset();

          if (!this.isModal)
          {
            this.alertService.showMessage("Login", `Welcome ${user.userName}!`, MessageSeverity.success);
          }
          else
          {
            setTimeout(() => {
              this.alertService.showStickyMessage("Session Restored", "Please try your last operation again", MessageSeverity.default);
            }, 10);

            this.closeModal();
          }
        }, 100);

        //REDIRECT to proper dashboard, if user logedin under some role
        if (user.roles) {
          this.redirectToRoleDefaultPage(user.roles[0]);
        }
      },
        error => {

          this.alertService.stopLoadingMessage();

          if (Utilities.checkNoNetwork(error)) {
            this.alertService.showStickyMessage(Utilities.noNetworkMessageCaption, Utilities.noNetworkMessageDetail, MessageSeverity.error, error);
            this.offerAlternateHost();
          }
          else {
            let errorMessage = Utilities.findHttpResponseMessage("error_description", error);

            if (errorMessage)
              this.showErrorAlert("Unable to login", errorMessage); //this.alertService.showStickyMessage("Unable to login", errorMessage, MessageSeverity.error, error);
            else
              this.alertService.showStickyMessage("Unable to login", "An error occured whilst logging in, please try again later.\nError: " + Utilities.getResponseBody(error), MessageSeverity.error, error);
          }

          setTimeout(() => {
            this.isLoading = false;
          }, 500);
        });
  }

  signUp() {
    this.authService.register();
  }

  offerAlternateHost() {

    if (Utilities.checkIsLocalHost(location.origin) && Utilities.checkIsLocalHost(this.configurations.baseUrl)) {
      this.alertService.showDialog("Dear Developer!\nIt appears your backend Web API service is not running...\n" +
        "Would you want to temporarily switch to the online Demo API below?(Or specify another)",
        DialogType.prompt,
        (value: string) => {
          this.configurations.baseUrl = value;
          this.alertService.showStickyMessage("API Changed!", "The target Web API has been changed to: " + value, MessageSeverity.warn);
        },
        null,
        null,
        null,
        this.configurations.fallbackBaseUrl);
    }
  }

  ngOnDestroy() {
    if (this.loginStatusSubscription)
      this.loginStatusSubscription.unsubscribe();
  }

  getShouldRedirect() {
    return !this.isModal && this.authService.isLoggedIn && !this.authService.isSessionExpired;
  }

  showErrorAlert(caption: string, message: string) {
    this.alertService.showMessage(caption, message, MessageSeverity.error);
  }

  closeModal() {
    if (this.modalClosedCallback) {
      this.modalClosedCallback();
    }
  }

  reset() {
    this.formResetToggle = false;

    setTimeout(() => {
      this.formResetToggle = true;
    });
  }
}
