import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Tenant } from 'src/app/models/tenant.model';
import { getBaseUrl } from 'src/app/app.module';
import { BrandNameEnum } from 'src/app/models/enums';

@Injectable()
export class CandidateService {

  //currentCandidateID: string;

  constructor(private http: HttpClient) { }

  getJobOffers(tenant: Tenant, candidateID: string)
  {
    let dataUrl = getBaseUrl();
    let tenantName = tenant != null ? tenant.name : BrandNameEnum.Default;

    if (candidateID)
    {
      dataUrl = dataUrl + `api/Candidate/${candidateID}/JobOffers`;
    }
    else
    {
      dataUrl = dataUrl + 'api/Candidate/AllJobOffers';
    }
    return this.http.get(dataUrl).pipe(map(response => response));
  }

  getContracts(tenant: Tenant, candidateID: string)
  {

    let dataUrl = getBaseUrl(); //baseUrl
    let tenantName = tenant != null ? tenant.name : BrandNameEnum.Default;

    if (candidateID)
    {
      dataUrl = dataUrl + `api/Candidate/${candidateID}/Contracts`; 
    }
    else
    {
      dataUrl = dataUrl + `api/${tenantName}/Consultant/AllContracts`;

      //dataUrl = dataUrl + 'api/Candidate/AllContracts/Tenant/' + tenant.name; //All contracts in the current tenant ?
      //dataUrl = dataUrl + 'api/Candidate/AllContracts'; //All contracts from different tenants ?
    }
    return this.http.get(dataUrl).pipe(map(response => response));
  }

  getDocuments(tenant: Tenant, candidateID: string) {
    let dataUrl = getBaseUrl(); //baseUrl

    if (candidateID) {
      dataUrl = dataUrl + `api/Candidate/${candidateID}/Documents`;
    }
    else {
      dataUrl = dataUrl + 'api/Candidate/AllDocuments';
    }

    // let headers = new Headers();
    // let token = localStorage.getItem('token');
    // headers.append('Authorization', 'Bearer' + token);
    //let options = new RequestOptions({ headers: headers });
    return this.http.get(dataUrl).pipe(map(response => response));
  }
}
