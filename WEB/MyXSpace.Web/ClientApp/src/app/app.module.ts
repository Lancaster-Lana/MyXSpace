import { NgModule, CUSTOM_ELEMENTS_SCHEMA, ErrorHandler, NO_ERRORS_SCHEMA } from "@angular/core";
import { FormGroup, FormArray, FormBuilder, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//Routing modules
import { RouterModule } from "@angular/router";
import { AppRoutingModule } from './app-routing.module';

//Translate modules 
import { TranslateModule, TranslateLoader, TranslatePipe } from "@ngx-translate/core";
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { AppTranslationService, TranslateLanguageLoader } from './services/app-translation.service';
//import { TranslateService, TranslatePipe, TRANSLATE_PROVIDERS } from 'ng2-translate/ng2-translate';

//Services (common -in shared.module)
import { AppErrorHandler } from './app-error.handler';

//MODULES of the application
import { SharedModule } from "./components/shared/shared.module";
import { AdminModule } from "./components/admin/admin.module";
import { ConsultantModule } from "./components/consultant/consultant.module";
import { CandidateModule } from "./components/candidate/candidate.module";
import { ClientModule } from "./components/client/client.module";

//Custom components
import { AdminLayoutComponent } from "./components/_layout/admin-layout.component";
import { ConsultantLayoutComponent } from "./components/_layout/consultant-layout.component";
import { CandidateLayoutComponent } from "./components/_layout/candidate-layout.component";
import { ClientLayoutComponent } from "./components/_layout/client-layout.component";

import { AppComponent } from "./components/app.component";
import { LoginComponent } from "./components/auth/login.component";
import { RegisterComponent } from "./components/auth/register.component";
import { HomeComponent } from "./components/home/home.component";

//COMMON ofter-used
import { BrandHeaderComponent } from "./components/shared/brand-header/brand-header.component";

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  imports: [
    RouterModule, AppRoutingModule,
    HttpClientModule, FormsModule, ReactiveFormsModule,
    BrowserModule, BrowserAnimationsModule,

    CommonModule, SharedModule.forRoot(),   //Shared module (with common components) 

    //TODO: must only in shared
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: TranslateLanguageLoader//useFactory: HttpLoaderFactory, deps: [HttpClient]
      }
    }),
    //Custom modules
    AdminModule, ClientModule, CandidateModule, ConsultantModule
  ],
  declarations: [
    AppComponent, BrandHeaderComponent,
    LoginComponent, RegisterComponent,
    HomeComponent,
    AdminLayoutComponent, ConsultantLayoutComponent, CandidateLayoutComponent, ClientLayoutComponent,
  ],
  providers: [
    AppTranslationService,  HttpClientModule,
    { provide: 'BASE_URL', useFactory: getBaseUrl },
    { provide: ErrorHandler, useClass: AppErrorHandler }
  ],
  exports: [TranslateModule ],
  bootstrap: [AppComponent] 
})
export class AppModule {
}

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

// required for AOT compilation
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
