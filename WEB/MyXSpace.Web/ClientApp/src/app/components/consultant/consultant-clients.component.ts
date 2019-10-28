import { Component } from '@angular/core';
import { SelectAllCheckboxState, PageChangeEvent } from '@progress/kendo-angular-grid';
import { Observable, of } from 'rxjs';
import { filter, map } from 'rxjs/operators';

import { ConsultantService } from './consultant.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';
import { AuthService } from '../../services/auth.service';
import { Utilities } from '../../services/utilities';

import { BrandNameEnum } from '../../models/enums';
import { Tenant } from '../../models/tenant.model';
import { Client } from '../../models/client.model';
import { RolesEnum } from '../../models/roles.enum';
import { InviteViewModel } from '../../models/invite.model';

@Component({
  templateUrl: './consultant-clients.component.html'
})
//Consultant clients list
export class ConsultantClientsListComponent {

  tenant: Tenant;
  brandName: string;

  consultantID: string; // current consultant - get from sesstion
  clients: Observable<Client[]>;   //clients$: Client[];  //consultant clients
  selectedClientCODEs: string[] = []; //selected clients to be invited for joining Brand\Tenant

  constructor(
    private consultantService: ConsultantService,
    private authService: AuthService,
    private alertService: AlertService) {
    this.tenant = authService.currentTenant; // current tenant - get from sesstion
    this.brandName = this.tenant != null ? this.tenant.name : BrandNameEnum.Default;

    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.consultantID = user != null ? user.id : null; //TODO: find consultant by name
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;

    this.clients = this.consultantService.getClients(this.tenant, this.consultantID);

    /*
    this.consultantService.getClients(this.tenant, this.consultantID)
      .subscribe((results: Array<any>) => {
        this.alertService.stopLoadingMessage(); //this.loadingIndicator = false;
        this.clients$ = results;
      },
      error => {
          this.alertService.stopLoadingMessage();//this.loadingIndicator = false;
          this.alertService.showStickyMessage("Load Error", `Unable to retrieve clients from the server.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        });*/
  }

  protected sendInvitations() {

    if (this.selectedClientCODEs) {

      //var selectedClients = this.clients$.filter(r => this.selectedClientIDs.includes(r.id));
      //let selectedClientsObs: Observable<Client[]> = this.clients.pipe(map((arr: Client[]) => arr.filter((cl: Client) => this.selectedClientIDs.includes(cl.id)))); //var selectedClients = this.clients.pipe(map(c => c), filter(cl => ...));
      let selectedClientsObs: Observable<Client[]> = this.clients.pipe(map(arr => arr.filter((cl: Client) => this.selectedClientCODEs.includes(cl.clientCode)))); 

      selectedClientsObs.toPromise()
        .then((selectedClients: Client[]) => {
          //send invitations if selected clients exists
          selectedClients.forEach(client => this.sendInvitationToClient(client))
        })
        .catch(error => {
          this.alertService.stopLoadingMessage();//this.loadingIndicator = false;
          this.alertService.showStickyMessage("Invite client error", `Unable to invite client.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        });
    }
  }

  protected sendInvitationToClient(client) {
    var model = new InviteViewModel(
      this.brandName, //tenantName
      RolesEnum.client, //role
      this.consultantID, //invitedBy
      //client.ID, //toID
      client.Email, //toEmail
      client.clientName //toName
    );

    this.consultantService.inviteClient(model);
      //.toPromise().then((result) => { //TDDO: check result of invite
      //  this.alertService.showMessage("Invitation sent to client !", `Invitation to join the tenant "${this.tenant.name}" has been sent sucessfully !`, MessageSeverity.success);
      //}).catch(error => {
      //  console.error(error);
      //  this.alertService.showStickyMessage("Invitation not sent to client", "The error ocuured:" + error.error, MessageSeverity.error, error.error);
      //});

      //TDDO: check result of invite(uncomment previous code - with catch)
      this.alertService.showMessage("Invitation sent to client !", `Invitation to join the tenant "${this.tenant.name}" has been sent sucessfully !`, MessageSeverity.success);
  }
}
