import { Injectable, Injector } from '@angular/core';
import { Router, NavigationExtras } from "@angular/router";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { DBkeys } from './db-Keys';
import { JwtHelper } from './jwt-helper';
import { AccountService } from './account.service';
import { ConfigurationService } from './configuration.service';
import { LocalStoreManager } from './local-store-manager.service';
import { EndpointFactory } from './endpoint-factory.service';
import { Utilities } from './utilities';
import { BrandNameEnum } from '../models/enums';
import { Tenant } from '../models/tenant.model';
import { User } from '../models/user.model';
import { LoginResponse, IdToken } from '../models/login-response.model';
import { PermissionValues } from '../models/permission.model';

@Injectable()
export class AuthService {

  constructor(private router: Router, private injector: Injector,
    private configurations: ConfigurationService,
    private httpClient: HttpClient,
    private localStorage: LocalStoreManager
    //private endpointFactory: EndpointFactory,
    //private accountService: AccountService, private sessionService: SessionService,
  ) {
    this.initializeLoginStatus();
  }

  _endpointFactory: EndpointFactory;
  private get endpointFactory() {
    if (!this._endpointFactory)
      this._endpointFactory = this.injector.get(EndpointFactory);
    return this._endpointFactory;
  }

  _accountService: AccountService;
  private get accountService() {
    if (!this._accountService)
      this._accountService = this.injector.get(AccountService);
    return this._accountService;
  }

  get currentTenant(): Tenant {

    //try get tenant from localstorage
    let tenant: Tenant = this.localStorage.getDataObject<Tenant>(DBkeys.CURRENT_TENANT);

    if (!tenant) {
      setTimeout(() => {
        var tenantObserv =this.accountService.currentTenant; // this.getTenantEndPoint(); ////

         //if (!tenantObserv)  return new Tenant(BrandNameEnum.Default); //init default tenant (as current absent yet !
        //else
        tenantObserv.toPromise()
          .then(result => {

            if (!result)
              alert("no tenant in config");
            else {
              tenant = result; //this.localStorage.getDataObject<Tenant>(DBkeys.CURRENT_TENANT);
              this.localStorage.saveSessionData(result, DBkeys.CURRENT_TENANT);
            }
        }, error => console.error(error));
      }, 500);

      tenant = this.localStorage.getDataObject<Tenant>(DBkeys.CURRENT_TENANT);
    }
    return tenant;
  }

  private initializeLoginStatus() {
    this.localStorage.getInitEvent().subscribe(() => {
      this.reevaluateLoginStatus();
    });
  }

  public get loginUrl() { return this.configurations.loginUrl; }
  public get homeUrl() { return this.configurations.homeUrl; }

  public loginRedirectUrl: string;
  public logoutRedirectUrl: string;

  public reLoginDelegate: () => void;

  private previousIsLoggedInCheck = false; //authenticated: boolean = false;
  private _loginStatus = new Subject<boolean>();

  get isLoggedIn(): boolean {
    return this.currentUser != null;
  }

  get currentUser(): User {

    let user = this.localStorage.getDataObject<User>(DBkeys.CURRENT_USER);
    this.reevaluateLoginStatus(user);
    return user;
  }

  get userPermissions(): PermissionValues[] {
    return this.localStorage.getDataObject<PermissionValues[]>(DBkeys.USER_PERMISSIONS) || [];
  }

  get idToken(): string {

    this.reevaluateLoginStatus();
    return this.localStorage.getData(DBkeys.ID_TOKEN);
  }

  get accessToken(): string {
    this.reevaluateLoginStatus();
    return this.localStorage.getData(DBkeys.ACCESS_TOKEN);
  }

  get refreshToken(): string {

    this.reevaluateLoginStatus();
    return this.localStorage.getData(DBkeys.REFRESH_TOKEN);
  }

  //get currentTockenUserInfo() {
  //  let token = localStorage.getItem('token');
  //  if (!token) return null;
  //  return new JwtHelper().decodeToken(token);
  //}

  get accessTokenExpiryDate(): Date {
    this.reevaluateLoginStatus();
    return this.localStorage.getDataObject<Date>(DBkeys.TOKEN_EXPIRES_IN, true);
  }

  get isSessionExpired(): boolean {
    if (this.accessTokenExpiryDate == null) {
      return true;
    }
    return !(this.accessTokenExpiryDate.valueOf() > new Date().valueOf());
  }

  get rememberMe(): boolean {
    return this.localStorage.getDataObject<boolean>(DBkeys.REMEMBER_ME) == true;
  }

  reevaluateLoginStatus(currentUser?: User) {

    let user = currentUser || this.localStorage.getDataObject<User>(DBkeys.CURRENT_USER);
    let isLoggedIn = user != null;

    if (this.previousIsLoggedInCheck != isLoggedIn) {
      setTimeout(() => {
        this._loginStatus.next(isLoggedIn);
      }, 500);
    }
    this.previousIsLoggedInCheck = isLoggedIn;
  }

  getLoginStatusEvent(): Observable<boolean> {
    return this._loginStatus.asObservable();
  }

  gotoPage(page: string, preserveParams = true) {

    let navigationExtras: NavigationExtras = {
      queryParamsHandling: preserveParams ? "merge" : "", preserveFragment: preserveParams
    };

    this.router.navigate([page], navigationExtras);
  }

  private saveUserDetails(user: User, permissions: PermissionValues[], accessToken: string, idToken: string, refreshToken: string, expiresIn: Date, rememberMe: boolean) {

    if (rememberMe) {
      this.localStorage.savePermanentData(accessToken, DBkeys.ACCESS_TOKEN);
      this.localStorage.savePermanentData(idToken, DBkeys.ID_TOKEN);
      this.localStorage.savePermanentData(refreshToken, DBkeys.REFRESH_TOKEN);
      this.localStorage.savePermanentData(expiresIn, DBkeys.TOKEN_EXPIRES_IN);
      this.localStorage.savePermanentData(permissions, DBkeys.USER_PERMISSIONS);
      this.localStorage.savePermanentData(user, DBkeys.CURRENT_USER);
    }
    else {
      this.localStorage.saveSyncedSessionData(accessToken, DBkeys.ACCESS_TOKEN);
      this.localStorage.saveSyncedSessionData(idToken, DBkeys.ID_TOKEN);
      this.localStorage.saveSyncedSessionData(refreshToken, DBkeys.REFRESH_TOKEN);
      this.localStorage.saveSyncedSessionData(expiresIn, DBkeys.TOKEN_EXPIRES_IN);
      this.localStorage.saveSyncedSessionData(permissions, DBkeys.USER_PERMISSIONS);
      this.localStorage.saveSyncedSessionData(user, DBkeys.CURRENT_USER);
    }
    this.localStorage.savePermanentData(rememberMe, DBkeys.REMEMBER_ME);
  }

  register() {
    this.router.navigate(['/register']);
  }

  //TODO: edirect to proper "role" page
  redirectLoginUser() {
    let redirect = this.loginRedirectUrl && this.loginRedirectUrl != '/'
      && this.loginRedirectUrl != ConfigurationService.defaultHomeUrl ? this.loginRedirectUrl : this.homeUrl;

    this.loginRedirectUrl = null;

    let urlParamsAndFragment = Utilities.splitInTwo(redirect, '#');
    let urlAndParams = Utilities.splitInTwo(urlParamsAndFragment.firstPart, '?');

    let navigationExtras: NavigationExtras = {
      fragment: urlParamsAndFragment.secondPart,
      queryParams: Utilities.getQueryParamsFromString(urlAndParams.secondPart),
      queryParamsHandling: "merge"
    };

    //TODO: navigate by role
    this.router.navigate([urlAndParams.firstPart], navigationExtras);
  }

  redirectLogoutUser() {
    let redirect = this.logoutRedirectUrl ? this.logoutRedirectUrl : this.loginUrl;
    this.logoutRedirectUrl = null;

    this.router.navigate([redirect]);
  }

  redirectForLogin() {
    this.loginRedirectUrl = this.router.url;
    this.router.navigate([this.loginUrl]);
  }

  login(userName: string, password: string, rememberMe?: boolean) {

    if (this.isLoggedIn)
      this.logout();

    return this.endpointFactory.getLoginEndpoint<LoginResponse>(userName, password)
      .pipe(map(response =>
        this.processLoginResponse(response, rememberMe)));
  }

  refreshLogin() {
    return this.endpointFactory.getRefreshLoginEndpoint<LoginResponse>().pipe(
      map(response => this.processLoginResponse(response, this.rememberMe)));
  }

  reLogin() {

    this.localStorage.deleteData(DBkeys.TOKEN_EXPIRES_IN);

    if (this.reLoginDelegate) {
      this.reLoginDelegate();
    }
    else {
      this.redirectForLogin();
    }
  }

  private processLoginResponse(response: LoginResponse, rememberMe: boolean) {

    let accessToken = response.access_token;

    if (accessToken == null)
      throw new Error("Received accessToken was empty");

    let idToken = response.id_token;

    let refreshToken = response.refresh_token || this.refreshToken;
    let expiresIn = response.expires_in;

    let tokenExpiryDate = new Date();
    tokenExpiryDate.setSeconds(tokenExpiryDate.getSeconds() + expiresIn);

    let accessTokenExpiry = tokenExpiryDate;

    let jwtHelper = new JwtHelper();
    let decodedIdToken = <IdToken>jwtHelper.decodeToken(response.id_token);

    let permissions: PermissionValues[] = Array.isArray(decodedIdToken.permission) ? decodedIdToken.permission : [decodedIdToken.permission];

    if (!this.isLoggedIn)
      this.configurations.import(decodedIdToken.configuration);

    let user = new User(
      decodedIdToken.sub,
      decodedIdToken.name,
      decodedIdToken.fullname,
      decodedIdToken.email,
      decodedIdToken.jobtitle,
      decodedIdToken.phone,
      Array.isArray(decodedIdToken.role) ? decodedIdToken.role : [decodedIdToken.role]);

    user.isEnabled = true;

    this.saveUserDetails(user, permissions, accessToken, idToken, refreshToken, accessTokenExpiry, rememberMe);

    this.reevaluateLoginStatus(user);

    return user;
  }

  logout(): void {
    this.localStorage.deleteData(DBkeys.ACCESS_TOKEN);
    this.localStorage.deleteData(DBkeys.ID_TOKEN);
    this.localStorage.deleteData(DBkeys.REFRESH_TOKEN);
    this.localStorage.deleteData(DBkeys.TOKEN_EXPIRES_IN);
    this.localStorage.deleteData(DBkeys.USER_PERMISSIONS);
    this.localStorage.deleteData(DBkeys.CURRENT_USER);

    this.configurations.clearLocalChanges();
    this.reevaluateLoginStatus();
  }
}
