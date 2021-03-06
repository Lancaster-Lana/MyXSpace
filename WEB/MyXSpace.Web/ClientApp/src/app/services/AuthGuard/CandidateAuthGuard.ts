import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { RolesEnum } from 'src/app/models/roles.enum';

//authorization check for 'external' role "Candidate"
@Injectable()
export class CandidateAuthGuard implements CanActivate {

    constructor(private authService: AuthService, private router: Router){ }

    canActivate()
    {
        if (this.authService.currentUser && this.authService.currentUser.roles.includes(RolesEnum.candidate))
        {
            // logged in so return true
            return true;
        }
     
        // not logged in so redirect to login page
        this.router.navigate(['/Login']);
        return false;
    }
}
