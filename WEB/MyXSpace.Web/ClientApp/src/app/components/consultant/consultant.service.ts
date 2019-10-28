import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { getBaseUrl } from 'src/app/app.module';
import { Tenant } from 'src/app/models/tenant.model';
import { Candidate } from 'src/app/models/candidate.model';
import { Observable } from 'rxjs';
import { Client } from 'src/app/models/client.model';
import { InviteViewModel } from 'src/app/models/invite.model';

@Injectable()
export class ConsultantService
{
  //currentConsultantID: string;
  constructor(private http: HttpClient) {}

  getCandidates(tenant: Tenant, consultantID: string): Observable<any[]>
  {
      let dataUrl = getBaseUrl(); //baseUrl

      if (consultantID) {    
        dataUrl = dataUrl + `api/Consultant/${consultantID}/Candidates`;
        //dataUrl = dataUrl + `api /{ tenantName }/Consultant/{ consultantID }/Candidates';  //dataUrl = dataUrl + `api/Consultant/${consultantID}/Candidates/Tenant/${tenant.name}`; //[Route("{consultantID}/Candidates/Tenant/{tenantID}")]
      }
      else {
        //dataUrl = dataUrl + 'api/Consultant/AllCandidates/Tenant/' + tenant.name;//show all candidates   [Route("AllCandidates/Tenant/{tenantID}")]
        dataUrl = dataUrl + 'api/Consultant/AllCandidates';
    }

    // let headers = new Headers();
    // let token = localStorage.getItem('token');
    // headers.append('Authorization', 'Bearer' + token);
    //let options = new RequestOptions({ headers: headers });
    return this.http.get<Candidate[]>(dataUrl).pipe(map(response => response));
  }

  getClients(tenant: Tenant, consultantID: string)
  {
    let dataUrl = getBaseUrl(); //baseUrl

    if (consultantID) {
      dataUrl = dataUrl + 'api/Consultant/' + consultantID + '/Clients';
    }
    else
    {
      dataUrl = dataUrl + 'api/Consultant/AllClients';//show all Clients if consultant is not defined
    }
    return this.http.get<Client[]>(dataUrl).pipe(map(response => response));
  }

  getContracts(tenant: Tenant, consultantID: string)
  {
    let dataUrl = getBaseUrl(); //baseUrl

    let tenantName = tenant != null ? tenant.name : "";

    if (consultantID) {
      dataUrl = dataUrl + `api/Consultant/${consultantID}/Contracts`;
      //dataUrl = dataUrl + `api/${tenantName}/Consultant/${consultantID}/Contracts`;
    }
    else {
      dataUrl = dataUrl + 'api/Consultant/AllContracts'; //+ '/Tenant/' + tenantName;
    }

    return this.http.get(dataUrl).pipe(map(response => response));
  }

  inviteCandidate(model: InviteViewModel)//invitedByConsultant: Consultant, candidate: Candidate, string roleName)
  {
    const inviteUrl: string = "/api/Invite/InviteCandidate";

    let dataUrl = getBaseUrl() + inviteUrl;//`api/${tenant.id}/Consultant/${invitedByConsultant.id}/Candidates/${candidate.id}/Invite/${roleName}`;
    
    return this.http.post(dataUrl, model).pipe(
      map(response =>
        response) //TODO: check response.status == true
      );
  }

  inviteClient(model: InviteViewModel)
  {
    var inviteUrl = "/api/Invite/InviteClient";

    let dataUrl = getBaseUrl() + inviteUrl;

    return this.http.post(dataUrl, model).pipe(
      map(response => response) //TODO: check response.status == true
    );
  }
}
