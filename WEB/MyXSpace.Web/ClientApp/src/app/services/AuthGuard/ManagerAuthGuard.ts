import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { RolesEnum } from 'src/app/models/roles.enum';

//authorization check for 'inner' role "Manager"
@Injectable()
export class ManagertAuthGuardService implements CanActivate {

    constructor(private authService: AuthService, private router: Router){}

    canActivate()
    {
      if (this.authService.currentUser && this.authService.currentUser.roles.includes(RolesEnum.manager))
        {
            // logged in so return true
            return true;
        }
        this.router.navigate(['/Login']);    // not logged in so redirect to login page
        return false;
    }
}
