import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { getBaseUrl } from 'src/app/app.module';
import { Tenant } from 'src/app/models/tenant.model';
import { BrandNameEnum } from 'src/app/models/enums';

@Injectable()
export class ClientService {

  constructor(private http: HttpClient) { }

  getJobOffers(tenant: Tenant, clientID: string)
  {
    let dataUrl = getBaseUrl();
    let tenantName = tenant != null ? tenant.name : BrandNameEnum.Default;

    if (clientID) {
      dataUrl = dataUrl + `api/Client/${clientID}/JobOffers`;
      //dataUrl = dataUrl + `api/${tenantName}/Client/${clientID}/JobOffers`;
    }
    else
    {
      dataUrl = dataUrl + 'api/Client/AllJobOffers';
      //dataUrl = dataUrl + `api/${tenantName}/Client/AllJobOffers`;
    }
    return this.http.get(dataUrl).pipe(map(response => response));
  }

  getContracts(tenant: Tenant, clientID: string) {

    let dataUrl = getBaseUrl(); //baseUrl
    let tenantName = tenant != null ? tenant.name : "";

    if (clientID) {
      dataUrl = dataUrl + `api/Client/${clientID}/Contracts`; 
    }
    else {
      //dataUrl = dataUrl + 'api/${tenantName}/Client/AllContracts'; //All contracts in the current tenant ?
      dataUrl = dataUrl + 'api/Client/AllContracts'; //All contracts from different tenants ?
    }
 
    return this.http.get(dataUrl).pipe(map(response => response));
  }

  getDocuments(tenant: Tenant, clientID: string) {
    let dataUrl = getBaseUrl(); //baseUrl

    if (clientID) {
      dataUrl = dataUrl + `api/Client/${clientID}/Documents`;
    }
    else {
      dataUrl = dataUrl + 'api/Client/AllDocuments';
    }

    // let headers = new Headers();
    // let token = localStorage.getItem('token');
    // headers.append('Authorization', 'Bearer' + token);
    //let options = new RequestOptions({ headers: headers });
    return this.http.get(dataUrl).pipe(map(response => response));
  }
}
