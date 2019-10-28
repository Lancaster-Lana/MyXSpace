import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from "@angular/router";
 
import { LoginComponent } from "./login.component";
import { RegisterComponent } from "./register.component";

import { AuthService } from "src/app/services/auth.service";
import { AuthGuard } from "src/app/services/AuthGuard/auth-guard.service";

@NgModule({
  imports: [CommonModule,BrowserModule, RouterModule, FormsModule, ReactiveFormsModule],
  declarations: [LoginComponent, RegisterComponent],
  providers: [ AuthService, AuthGuard],
  exports: [ LoginComponent, RegisterComponent]
})
export class AuthModule { }
