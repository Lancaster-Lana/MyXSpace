import { Observable, Subscription, fromEvent } from 'rxjs';
import {CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA,  NgModule, ModuleWithProviders,
  Directive, ElementRef, Input, Output, EventEmitter, OnInit, OnDestroy, ErrorHandler} from '@angular/core';
declare var $: any; //NOTE: for support Jquery directives\components

import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { AngularFontAwesomeModule } from 'angular-font-awesome';

//Translate modules \services
import { TranslateModule, TranslateLoader, TranslatePipe } from "@ngx-translate/core";
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
//import { TranslateService, TranslatePipe, TRANSLATE_PROVIDERS } from 'ng2-translate/ng2-translate';
import { AppTranslationService, TranslateLanguageLoader } from 'src/app/services/app-translation.service';

//Custom services (providers)
import { AppErrorHandler } from 'src/app/app-error.handler';
import { AlertService } from 'src/app/services/alert.service';
import { NotificationService } from 'src/app/services/notification.service';
import { NotificationEndpoint } from 'src/app/services/notification-endpoint.service';
import { Utilities } from 'src/app/services/utilities';
import { AccountService } from 'src/app/services/account.service';
import { AccountEndpoint } from 'src/app/services/account-endpoint.service';
import { AppTitleService } from 'src/app/services/app-title.service';
import { ConfigurationService } from 'src/app/services/configuration.service';
import { LocalStoreManager } from 'src/app/services/local-store-manager.service';
import { EndpointFactory } from 'src/app/services/endpoint-factory.service';

//KENDO UI
import { GridModule } from '@progress/kendo-angular-grid';
import { DropDownsModule } from "@progress/kendo-angular-dropdowns";

//NGX
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PopoverModule } from "ngx-bootstrap/popover";
import { TooltipModule } from "ngx-bootstrap/tooltip";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { ChartsModule } from 'ng2-charts';

//cuostom Directives, pipes
import { GroupByPipe } from 'src/app/pipes/group-by.pipe';
import { EqualValidator } from 'src/app/directives/equal-validator.directive';
import { LastElementDirective } from 'src/app/directives/last-element.directive';
import { AutofocusDirective } from 'src/app/directives/autofocus.directive';
//import { BootstrapSelectDirective } from 'src/app/directives/bootstrap-select.directive';
//import { BootstrapTabDirective } from 'src/app/directives/bootstrap-tab.directive';
//import { BootstrapToggleDirective } from 'src/app/directives/bootstrap-toggle.directive';
//import { BootstrapDatepickerDirective } from 'src/app/directives/bootstrap-datepicker.directive';

//Custom shared components
import { UserPreferencesComponent } from '../admin/user-preferences.component';
import { UserInfoComponent } from '../admin/user-info.component';
import { SettingsComponent } from './settings/settings.component';
import { StatisticsDemoComponent } from './statistics/statistics-demo.component';
import { SearchBoxComponent } from './search/search-box.component';
import { NotFoundComponent } from "../../components/shared/not-found/not-found.component";
import { AboutComponent } from "../../components/about/about.component";
import { NotificationsViewerComponent } from "../../components/shared/notifications/notifications-viewer.component";

@NgModule({
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  imports: [
    HttpClientModule, RouterModule,
    FormsModule, ReactiveFormsModule, 
    AngularFontAwesomeModule,
    CommonModule, BrowserModule, BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-top-right',
      preventDuplicates: true
    }),
    ModalModule.forRoot(),
    PopoverModule.forRoot(),
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    CarouselModule.forRoot(),
    NgxDatatableModule,
    ChartsModule,
    GridModule, DropDownsModule, //KENDO UI

    TranslateModule.forRoot({ //.forChild ?
      loader: {
        provide: TranslateLoader,
        useClass: TranslateLanguageLoader//useFactory: HttpLoaderFactory, deps: [HttpClient]
      }
    }),
  ],
  declarations: [ //ROUTER_DIRECTIVES,
    //Pipes, Directives:
    GroupByPipe,
    EqualValidator, LastElementDirective, AutofocusDirective,
    //BootstrapSelectDirective, BootstrapTabDirective, BootstrapToggleDirective, BootstrapDatepickerDirective,
    //Custom components:
    AboutComponent, SearchBoxComponent, NotFoundComponent, NotificationsViewerComponent,
    UserPreferencesComponent, UserInfoComponent,
    SettingsComponent, StatisticsDemoComponent
  ],
  providers: [
    { provide: 'BASE_URL', useFactory: getBaseUrl }, // { provide: PLATFORM_PIPES, useValue: TranslatePipe, multi: true },
    { provide: ErrorHandler, useClass: AppErrorHandler },
    AppTranslationService, ToastrService,
    AccountService,  LocalStoreManager, EndpointFactory,
    ConfigurationService, AppTitleService,
    AlertService, NotificationService, NotificationEndpoint,
    Utilities
  ],
  exports: [
    //Share modules
    TranslateModule,
    AngularFontAwesomeModule,

    //KENDO UI
    GridModule, DropDownsModule,
    //NGX UI
    ToastrModule, ModalModule, TooltipModule, PopoverModule, BsDropdownModule, CarouselModule ,
    ChartsModule, NgxDatatableModule, //filterable tables

    //Share pipes and directives
     GroupByPipe, EqualValidator,    LastElementDirective, AutofocusDirective,
    //BootstrapSelectDirective, BootstrapTabDirective, BootstrapToggleDirective, BootstrapDatepickerDirective,

    //Share custom components (via all modules)
    AboutComponent, SearchBoxComponent, NotFoundComponent, NotificationsViewerComponent,
    UserPreferencesComponent, UserInfoComponent, 
    SettingsComponent, StatisticsDemoComponent
  ]
})

export class SharedModule {

  static forRoot(): ModuleWithProviders { //https://alligator.io/angular/providers-shared-modules/
    return {
      ngModule: SharedModule,

      //Some LazyLoading Services
      providers: [AppTranslationService, ToastrService,
        ConfigurationService,  LocalStoreManager, EndpointFactory,
        AccountService, AccountEndpoint,
        AppTitleService, 
        AlertService, NotificationService, NotificationEndpoint, AppTitleService,
        Utilities
      ] 
    };
  }

  //static forChild(): ModuleWithProviders {
  //  return {
  //    ngModule: LanguagesModule,
  //    providers: [
  //      LanguagesService,
  //      TranslateModule.forChild({ loader: { provide: TranslateLoader, useClass: LanguageLoader } }).providers
  //    ]
  //  };
  //}
}

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

// required for AOT compilation
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}
