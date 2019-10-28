import { Injectable, Injector } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, Subject, throwError } from 'rxjs';
import { mergeMap, switchMap, catchError } from 'rxjs/operators';

import { AuthService } from './auth.service';
import { ConfigurationService } from './configuration.service';

@Injectable()
export class EndpointFactory
{
  static readonly apiVersion: string = "1";

  private taskPauser: Subject<any>;
  private isRefreshingLogin: boolean;

  constructor(protected http: HttpClient, protected configurations: ConfigurationService, private injector: Injector) { }

  private _authService: AuthService;
  get authService() {
    if (!this._authService)
      this._authService = this.injector.get(AuthService);

    return this._authService;
  }

  private readonly _loginUrl: string = "/connect/token";
  get loginUrl() {
    return this.configurations.baseUrl + this._loginUrl;
  }

  getLoginEndpoint<T>(userName: string, password: string): Observable<T> {

    let header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    let params = new HttpParams()
          .append('username', userName)
          .append('password', password)
          .append('grant_type', 'password')
          .append('scope', 'openid email phone profile offline_access roles');

      //NEW
      /*const credentials = {
          username: userName,
          password: password
      };
      let paramsNew = new HttpParams()
          .append('client_id', 'angular_spa')
          .append('grant_type', 'authorization_code')
          .append('redirect_uri', 'http://localhost:56876/callback') //OWN Client app
          .append('scope', 'read')
          .append('response_type', 'code');
      //params.append('state', this.state);
      */

    let requestBody = params.toString();

    return this.http.post<T>(this.loginUrl, requestBody, { headers: header });
  }

/* export function getClientSettings(): UserManagerSettings {
 return {
     authority: aipServer,
     client_id: 'angular_spa',
     response_type: "id_token token",
     scope: "openid profile email api.read",
     filterProtocolClaims: true,
     loadUserInfo: true,
     automaticSilentRenew: true,
     redirect_uri: redirectURL,
     post_logout_redirect_uri: 'http://localhost:4200/',
     silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
 };
}*/


  getRefreshLoginEndpoint<T>(): Observable<T> {

    let header = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    let params = new HttpParams()
      .append('refresh_token', this.authService.refreshToken)
      .append('grant_type', 'refresh_token')
      .append('scope', 'openid email phone profile offline_access roles');

    let requestBody = params.toString();

    return this.http.post<T>(this.loginUrl, requestBody, { headers: header }).pipe<T>(
      catchError(error => {
        return this.handleError(error, () => this.getRefreshLoginEndpoint());
      }));
  }

  /*protected*/ getRequestHeaders(): { headers: HttpHeaders | { [header: string]: string | string[]; } } {
    let headers = new HttpHeaders({
      'Authorization': 'Bearer ' + this.authService.accessToken,
      'Content-Type': 'application/json',
      'Accept': `application/vnd.iman.v${EndpointFactory.apiVersion}+json, application/json, text/plain, */*`,
      'App-Version': ConfigurationService.appVersion
    });

    return { headers: headers };
    }


    /*
    // Code Flow with PCKE
    requestTokensWithCodeProcedure(code: string, state: string, session_state: string | null) {
        let tokenRequestUrl = '';
        if (this.configurationProvider.wellKnownEndpoints && this.configurationProvider.wellKnownEndpoints.token_endpoint) {
            tokenRequestUrl = `${this.configurationProvider.wellKnownEndpoints.token_endpoint}`;
        }

        if (!this.oidcSecurityValidation.validateStateFromHashCallback(state, this.oidcSecurityCommon.authStateControl)) {
            this.loggerService.logWarning('authorizedCallback incorrect state');
            // ValidationResult.StatesDoNotMatch;
            return;
        }

        let headers: HttpHeaders = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/x-www-form-urlencoded');

        let data =
            `grant_type=authorization_code&client_id=${this.configurationProvider.openIDConfiguration.client_id}` +
            `&code_verifier=${this.oidcSecurityCommon.code_verifier}&code=${code}&redirect_uri=${
            this.configurationProvider.openIDConfiguration.redirect_url
            }`;
        if (this.oidcSecurityCommon.silentRenewRunning === 'running') {
            data =
                `grant_type=authorization_code&client_id=${this.configurationProvider.openIDConfiguration.client_id}` +
                `&code_verifier=${this.oidcSecurityCommon.code_verifier}&code=${code}&redirect_uri=${
                this.configurationProvider.openIDConfiguration.silent_renew_url
                }`;
        }

        this.httpClient
            .post(tokenRequestUrl, data, { headers: headers })
            .pipe(
                map(response => {
                    let obj: any = new Object();
                    obj = response;
                    obj.state = state;
                    obj.session_state = session_state;

                    this.authorizedCodeFlowCallbackProcedure(obj);
                }),
                catchError(error => {
                    this.loggerService.logError(error);
                    this.loggerService.logError(`OidcService code request ${this.configurationProvider.openIDConfiguration.stsServer}`);
                    return of(false);
                })
            )
            .subscribe();
    }*/

  protected handleError(error, continuation: () => Observable<any>) {

    if (error.status == 401) {
      if (this.isRefreshingLogin) {
        return this.pauseTask(continuation);
      }

      this.isRefreshingLogin = true;

      return this.authService.refreshLogin().pipe(
        mergeMap(data => {
          this.isRefreshingLogin = false;
          this.resumeTasks(true);

          return continuation();
        }),
        catchError(refreshLoginError => {
          this.isRefreshingLogin = false;
          this.resumeTasks(false);

          if (refreshLoginError.status == 401 || (refreshLoginError.url && refreshLoginError.url.toLowerCase().includes(this.loginUrl.toLowerCase()))) {
            this.authService.reLogin();
            return throwError('session expired');
          }
          else {
            return throwError(refreshLoginError || 'server error');
          }
        }));
    }

    if (error.url && error.url.toLowerCase().includes(this.loginUrl.toLowerCase())) {
      this.authService.reLogin();

      return throwError((error.error && error.error.error_description) ? `session expired (${error.error.error_description})` : 'session expired');
    }
    else {
      return throwError(error);
    }
  }

  private pauseTask(continuation: () => Observable<any>) {
    if (!this.taskPauser)
      this.taskPauser = new Subject();

    return this.taskPauser.pipe(switchMap(continueOp => {
      return continueOp ? continuation() : throwError('session expired');
    }));
  }

  private resumeTasks(continueOp: boolean) {
    setTimeout(() => {
      if (this.taskPauser) {
        this.taskPauser.next(continueOp);
        this.taskPauser.complete();
        this.taskPauser = null;
      }
    });
  }
}
