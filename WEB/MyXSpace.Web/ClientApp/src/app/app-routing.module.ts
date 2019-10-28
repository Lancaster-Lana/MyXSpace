import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { NotFoundComponent } from './components/shared/not-found/not-found.component';
import { SettingsComponent } from "./components/shared/settings/settings.component";
import { AboutComponent } from "./components/about/about.component";

import { HomeComponent } from "./components/home/home.component";
import { LoginComponent } from './components/auth/login.component';
import { RegisterComponent } from './components/auth/register.component';

import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/AuthGuard/auth-guard.service';
import { UserPreferencesComponent } from './components/admin/user-preferences.component';

//Admin Board
import { AdminLayoutComponent } from './components/_layout/admin-layout.component';
import { UsersManagementComponent } from './components/admin/users-management.component';
import { RolesManagementComponent } from './components/admin/roles-management.component';

//Consultant Board
import { ConsultantLayoutComponent } from './components/_layout/consultant-layout.component';
import { ConsultantDashboardComponent } from './components/consultant/consultant-dashboard.component';
import { ConsultantCandidatesListComponent } from './components/consultant/consultant-candidates.component';
import { ConsultantClientsListComponent } from './components/consultant/consultant-clients.component';
import { ConsultantContractsListComponent } from './components/consultant/consultant-contracts.component';
import { ConsultantFAQComponent } from './components/consultant/consultantFAQ.component';
import { ConsultantReportingComponent } from './components/consultant/consultant-reporting.component';

//Candidate board
import { CandidateLayoutComponent } from './components/_layout/candidate-layout.component';
import { CandidateDashboardComponent } from './components/candidate/candidate-dashboard.component';
import { CandidateContractsListComponent } from './components/candidate/candidate-contracts.component';
import { CandidateJobOffersComponent } from './components/candidate/candidate-JobOffers.component';
import { CandidateDocumentsComponent } from './components/candidate/candidate-documents.component';

//Client board
import { ClientLayoutComponent } from './components/_layout/client-layout.component';
import { ClientDashboardComponent } from './components/client/client-dashboard.component';
import { ClientJobOffersComponent } from './components/client/client-jobOffers.component';
import { ClientContractsListComponent } from './components/client/client-contracts.component';

const routes: Routes = [
  { path: "", component: HomeComponent, data: { title: "Home" } },
  { path: "about", component: AboutComponent, data: { title: "About Us" } },
  { path: "home", redirectTo: "/", pathMatch: "full" },

  { path: "login", component: LoginComponent, data: { title: "Login" } },
  { path: "register", component: RegisterComponent }, //{ path: "logout", component: Logout },
  { path: "preferences", component: UserPreferencesComponent, canActivate: [AuthGuard], data: { title: "Preferences" } },
  { path: "settings", component: SettingsComponent, canActivate: [AuthGuard], data: { title: "Settings" } },
  {
    path: 'consultant',
    component: ConsultantLayoutComponent,
    //canActivate: [ConsultantAuthGuard]
    children: [
      //{ path: 'MyProfile', component: MyConsultantComponent },
      { path: "candidates", component: ConsultantCandidatesListComponent },
      { path: "clients", component: ConsultantClientsListComponent },
      { path: "contracts", component: ConsultantContractsListComponent }, //{ path: 'contract/:contractID', component: ContractEditComponent, canActivate: [ContractGuardService] }
      { path: "reporting", component: ConsultantReportingComponent },
      { path: "consultantfaq", component: ConsultantFAQComponent },
      { path: '', component: ConsultantDashboardComponent }
    ]
  },
  {
    path: 'candidate',
    component: CandidateLayoutComponent,
    //canActivate: [CandidateAuthGuard] TODO:
    children: [
          { path: "joboffers", component: CandidateJobOffersComponent },
          { path: "documents", component: CandidateDocumentsComponent },
          { path: "contracts", component: CandidateContractsListComponent },
          //{ path: 'candidateProfile', component: MyCandidateProfileComponent },
          //{ path: "financial", component: CandidateFinancialComponent },
          //{ path: "leaveTestimonial", component: CandidateLeaveTestimonialComponent },
          { path: '', component: CandidateDashboardComponent }
    ]
  },
  {
    path: 'client',
    component: ClientLayoutComponent,
  //  canActivate: [ClientAuthGuardService]
    children: [
     //{ path: 'myProfile', component: MyClientProfileComponent },
      { path: 'dashboard', component: ClientDashboardComponent },
      { path: "jobOffers", component: ClientJobOffersComponent },
      { path: "contracts", component: ClientContractsListComponent },  
      //{ path: "documents", component: ClientDocumentsManagementComponent },
    ]
  },
  {
    path: "admin", //TODO:lazy load path: 'admin', loadChildren: './ modules / admin / admin.module#AdminModule’}
    component: AdminLayoutComponent,
    //canActivateChild: [AdminAuthGuard],
    children: [
      { path: "roles", component: RolesManagementComponent },
      { path: "users", component: UsersManagementComponent },
      { path: "", component: UsersManagementComponent }
    ]
  },

  { path: "**", component: NotFoundComponent, data: { title: "Page Not Found" } }
];

@NgModule({
  imports: [RouterModule.forRoot(routes), TranslateModule],
  exports: [RouterModule],
  providers: [AuthService, AuthGuard]
})
export class AppRoutingModule { }
