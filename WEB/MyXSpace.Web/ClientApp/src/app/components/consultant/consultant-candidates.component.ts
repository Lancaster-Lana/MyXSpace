import { Component, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';
import { ConsultantService } from './consultant.service';
import { BrandNameEnum } from '../../models/enums';
import { Tenant } from '../../models/tenant.model';
import { Candidate } from '../../models/candidate.model';
import { Utilities } from 'src/app/services/utilities';
import { RolesEnum } from 'src/app/models/roles.enum';
import { InviteViewModel } from 'src/app/models/invite.model';

@Component({
  templateUrl: './consultant-candidates.component.html'
})
export class ConsultantCandidatesListComponent {
  tenant: Tenant;
  brandName: string;

  consultantID: string; // current consultant : get from sesstion or route
  candidates: Observable<Candidate[]>; //candidates$://list of candidates of the current consultant
  selectedCandidatesEMAILs: string[] = [];//selected candidates to be invited to join Brand\Tenant

  constructor(private consultantService: ConsultantService, private authService: AuthService, private alertService: AlertService) {
    var user = authService.currentUser;  //activeRoute.snapshot.params["consultantID"]; //id from route or session
    this.consultantID = user != null ? user.id : null; //TODO: find consultant by name

    this.tenant = authService.currentTenant;
    this.brandName = this.tenant != null ? this.tenant.name : BrandNameEnum.Default;
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {

    this.alertService.startLoadingMessage(); //this.loadingIndicator = true;
    this.candidates = this.consultantService.getCandidates(this.tenant, this.consultantID);
  }

  protected sendInvitations() {

    if (this.selectedCandidatesEMAILs) {
      var selectedCandidates = this.candidates.pipe(map(arr => arr.filter(c => this.selectedCandidatesEMAILs.includes(c.email)))); //var selectedClients = this.clients.pipe(map(c => c), filter(cl => ...));
      selectedCandidates.toPromise().then(candidates => {
        if (candidates) {
          //send invitations if selected clients exists
          candidates.forEach(c => this.sendInvitationToCandidate(c)); //c.id, c.email, c.firstName + ' ' + c.lastName));
        }
      })
        .catch(error => {
          this.alertService.stopLoadingMessage();//this.loadingIndicator = false;
          this.alertService.showStickyMessage("Invite error", `Unable to invite candidate.\r\nErrors: "${Utilities.getHttpResponseMessage(error)}"`,
            MessageSeverity.error, error);
        });
    }
  }

  protected sendInvitationToCandidate(candidate)
  {
    var model = new InviteViewModel(
      this.tenant != null ? this.tenant.name : BrandNameEnum.Default,
      RolesEnum.candidate, //TODO: RoleEnum.TW etc
      this.consultantID,
      //candidate.ID, //toID
      candidate.Email, //toEmail
      candidate.firstName + ' ' + candidate.lastName//toName
    );

    this.consultantService.inviteCandidate(model).toPromise();
    /*.then((result) => { //TDDO: check result of inviteCandidate
       this.alertService.showMessage("Invitations sent to candidate !", `Invitation to join the tenant "${this.tenant.name}" has been sent sucessfully !`, MessageSeverity.success);
     })
    .catch(error => {

      console.error(error);
      this.alertService.showStickyMessage("Invitation not sent to candidate", "The error ocuured:" + error.error, MessageSeverity.error, error.error);
    });*/

    //TDDO: check result of invite(uncomment previous code - with catch)
    this.alertService.showMessage("Invitation sent to candidate !", `Invitation to join the tenant "${this.tenant.name}" has been sent sucessfully !`, MessageSeverity.success);
  }
}
