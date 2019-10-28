import { Component, ViewEncapsulation, OnInit, OnDestroy, ViewChildren, AfterViewInit, QueryList, ElementRef } from "@angular/core";
import { Router, NavigationStart } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ToastrService, ToastrConfig, ToastRef, ActiveToast } from 'ngx-toastr';

import { LoginComponent } from "./auth/login.component";
import { AlertService, AlertDialog, DialogType, AlertMessage, MessageSeverity } from '../services/alert.service';
import { NotificationService } from "../services/notification.service";

import { LocalStoreManager } from '../services/local-store-manager.service';
import { AppTitleService } from '../services/app-title.service';
import { ConfigurationService } from '../services/configuration.service';
import { AppTranslationService } from "../services/app-translation.service";

import { AuthService } from '../services/auth.service';
import { AccountService } from '../services/account.service';

import { Permission } from '../models/permission.model';
import { Tenant } from "../models/tenant.model";

var alertify: any = require('../assets/scripts/alertify.js');

@Component({
  selector: "app-root",
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit//, AfterViewInit
{
  isAppLoaded: boolean;

  isUserLoggedIn: boolean;
  tenant: Tenant;

  shouldShowLoginModal: boolean;
  removePrebootScreen: boolean;
  newNotificationCount = 0;

  appTitle = "MyXSpace"; //NOTE: current brand\tenant will be detected in run-time
  appLogo = require("../assets/images/logo-default.png");

  stickyToasties: number[] = [];

  dataLoadingConsecutiveFailurs = 0;
  notificationsLoadingSubscription: any;

  //@ViewChildren('loginModal,loginControl')
  //modalLoginControls: QueryList<any>;

  loginModal: ModalDirective;
  loginControl: LoginComponent;

  get fullName(): string {
    return this.authService.currentUser ? this.authService.currentUser.fullName : "";
  }

  get userName(): string {
    return this.authService.currentUser ? this.authService.currentUser.userName : "";
  }

  get canViewClients() {
    return this.accountService.userHasPermission(Permission.viewUsersPermission); //eg. viewClientsPermission
  }

  get canViewCandidates() {
    return this.accountService.userHasPermission(Permission.viewUsersPermission); //eg. viewCandidatesPermission
  }

  getYear() {
    return new Date().getUTCFullYear();
  }

  constructor(
    private accountService: AccountService,private authService: AuthService,  private storageManager: LocalStoreManager, 
    private toastrService: ToastrService,private alertService: AlertService, private notificationService: NotificationService,
    private appTitleService: AppTitleService,
    private translationService: AppTranslationService, public configurations: ConfigurationService, public router: Router) {

    //Set languages support
    translationService.addLanguages(["en", "fr", "de"]);
    translationService.setDefaultLanguage('en');

    storageManager.initialiseStorageSyncListener();
  }

  ngOnInit() {

    //Set current Brand\tenant
    this.tenant = this.authService.currentTenant;
    this.appTitleService.appName = this.appTitle;

    if (this.tenant && this.tenant.name) {
      this.appLogo = require(`../assets/images/logo-${this.tenant.name}.png`);
    }

    this.isUserLoggedIn = this.authService.isLoggedIn;

    // 1 sec to ensure all the effort to get the css animation working is appreciated :|, Preboot screen is removed .5 sec later
    setTimeout(() => this.isAppLoaded = true, 700);
    setTimeout(() => this.removePrebootScreen = true, 1000);

    setTimeout(() => {
      if (this.isUserLoggedIn) {
        this.alertService.resetStickyMessage();

        if (this.authService.isSessionExpired)
          this.alertService.showStickyMessage("Session Expired", "Your Session has expired. Please log in again", MessageSeverity.warn);
        //else this.alertService.showMessage("Login", `Welcome back ${this.userName}!`, MessageSeverity.default);
      }
    }, 2000);

    this.alertService.getDialogEvent().subscribe(alert => this.showDialog(alert));
    this.alertService.getMessageEvent().subscribe(message => this.showToast(message, false));
    this.alertService.getStickyMessageEvent().subscribe(message => this.showToast(message, true));

    this.authService.reLoginDelegate = () => this.shouldShowLoginModal = true;

    this.authService.getLoginStatusEvent().subscribe(isLoggedIn => {

      this.isUserLoggedIn = isLoggedIn;

      if (this.isUserLoggedIn) {
        this.initNotificationsLoading();
      }
      else {
        this.unsubscribeNotifications();
      }

      setTimeout(() => {
        if (!this.isUserLoggedIn) {
          this.alertService.showMessage("Session Ended!", "", MessageSeverity.default);
        }
      }, 500);
    });

    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        let url = (<NavigationStart>event).url;

        if (url !== url.toLowerCase()) {
          this.router.navigateByUrl((<NavigationStart>event).url.toLowerCase());
        }
      }
    });
  }

  ngOnDestroy() {
    this.unsubscribeNotifications();
  }

  ngAfterViewInit() {
    //this.modalLoginControls.changes.subscribe((controls: QueryList<any>) => {
    //  controls.forEach(control => {
    //    if (control) {
    //      if (control instanceof LoginComponent) {
    //        this.loginControl = control;
    //        this.loginControl.modalClosedCallback = () => this.loginModal.hide();
    //      }
    //      else {
    //        this.loginModal = control;
    //        this.loginModal.show();
    //      }
    //    }
    //  });
    //});
  }

  logout() {
    this.authService.logout();
    this.authService.redirectLogoutUser();
  }

  get notificationsTitle() {

    let gT = (key: string) => this.translationService.getTranslation(key);

    if (this.newNotificationCount)
      return `${gT("app.Notifications")} (${this.newNotificationCount} ${gT("app.New")})`;
    else
      return gT("app.Notifications");
  }

  onLoginModalShown() {
    this.alertService.showStickyMessage("Session Expired", "Your Session has expired. Please log in again", MessageSeverity.info);
  }

  onLoginModalHidden() {
    this.alertService.resetStickyMessage();
    this.loginControl.reset();
    this.shouldShowLoginModal = false;

    if (this.authService.isSessionExpired)
      this.alertService.showStickyMessage("Session Expired", "Your Session has expired. Please log in again to renew your session", MessageSeverity.warn);
  }

  onLoginModalHide() {
    this.alertService.resetStickyMessage();
  }

  private unsubscribeNotifications() {
    if (this.notificationsLoadingSubscription)
      this.notificationsLoadingSubscription.unsubscribe();
  }

  initNotificationsLoading() {

    this.notificationsLoadingSubscription = this.notificationService.getNewNotificationsPeriodically()
      .subscribe(notifications => {
        this.dataLoadingConsecutiveFailurs = 0;
        this.newNotificationCount = notifications.filter(n => !n.isRead).length;
      },
        error => {
          this.alertService.logError(error);

          if (this.dataLoadingConsecutiveFailurs++ < 20)
            setTimeout(() => this.initNotificationsLoading(), 5000);
          else
            this.alertService.showStickyMessage("Load Error", "Loading new notifications from the server failed!", MessageSeverity.error);
        });
  }

  markNotificationsAsRead()
  {
    let recentNotifications = this.notificationService.recentNotifications;

    if (recentNotifications.length) {
      this.notificationService.readUnreadNotification(recentNotifications.map(n => n.id), true)
        .subscribe(response => {
          for (let n of recentNotifications) {
            n.isRead = true;
          }
          this.newNotificationCount = recentNotifications.filter(n => !n.isRead).length;
        },
          error => {
            this.alertService.logError(error);
            this.alertService.showMessage("Notification Error", "Marking read notifications failed", MessageSeverity.error);
        });
    }
  }

  showDialog(dialog: AlertDialog) {

    alertify.set({
      labels: {
        ok: dialog.okLabel || "OK",
        cancel: dialog.cancelLabel || "Cancel"
      }
    });

    switch (dialog.type) {
      case DialogType.alert:
        alertify.alert(dialog.message);
        break
      case DialogType.confirm:
        alertify
          .confirm(dialog.message, (e) => {
            if (e) {
              dialog.okCallback();
            }
            else {
              if (dialog.cancelCallback)
                dialog.cancelCallback();
            }
          });

        break;
      case DialogType.prompt:
        alertify
          .prompt(dialog.message, (e, val) => {
            if (e) {
              dialog.okCallback(val);
            }
            else {
              if (dialog.cancelCallback)
                dialog.cancelCallback();
            }
          }, dialog.defaultValue);

        break;
    }
  }

  showToast(message: AlertMessage, isSticky: boolean) {

    if (message == null) {
      for (let id of this.stickyToasties.slice(0)) {
        //this.toastaService.clear(id);
      }
      return;
    }

    switch (message.severity) {
      case MessageSeverity.default:
        this.toastrService.info(message.detail, message.summary); //TODO:
        break;
      case MessageSeverity.info:
        this.toastrService.info(message.detail, message.summary);
        break;
      case MessageSeverity.success:
        this.toastrService.success(message.detail, message.summary);
        break;
      case MessageSeverity.error:
        this.toastrService.error(message.detail, message.summary);
        break;
      case MessageSeverity.warn:
        this.toastrService.warning(message.detail, message.summary);
        break;
      case MessageSeverity.wait:
        //this.toastaService.wait(toastOptions);
        break;
    }
  }
}
