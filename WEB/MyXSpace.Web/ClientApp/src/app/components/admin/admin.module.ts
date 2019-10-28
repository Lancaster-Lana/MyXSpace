import { NgModule, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from "@angular/platform-browser"
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { ModalModule } from 'ngx-bootstrap/modal';
import { TranslateModule, TranslateLoader, TranslatePipe } from "@ngx-translate/core";
import { AppTranslationService, TranslateLanguageLoader } from '../../services/app-translation.service';
import { SharedModule } from "../../components/shared/shared.module";

//Administrative components
//import { AdminLayoutComponent } from '../_layout/admin-layout.component'; 
import { UsersManagementComponent } from './users-management.component';
import { RolesManagementComponent } from './roles-management.component';
import { RoleEditorComponent } from './role-editor.component';

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  declarations: [
    UsersManagementComponent, 
    RolesManagementComponent, RoleEditorComponent
  ],

  imports: [
    BrowserModule, FormsModule, RouterModule,
    ModalModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: TranslateLanguageLoader
      },
    }),
    SharedModule, //with custom directives, pipes, services
  ],
  providers: [AppTranslationService], //export services

  exports: [
    UsersManagementComponent, RolesManagementComponent, RoleEditorComponent,
  ]
})
export class AdminModule { }
