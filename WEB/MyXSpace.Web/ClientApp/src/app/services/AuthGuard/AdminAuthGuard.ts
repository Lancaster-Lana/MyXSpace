import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Injectable()
export class AdminAuthGuard implements CanActivate {

    constructor(private authService: AuthService, private router: Router) { }

    canActivate()
    {
        if (this.authService.currentUser && this.authService.currentUser.roles.includes("admin"))// if (localStorage.getItem('AdminUser'))
        {
            return true;
        }
        this.router.navigate(['/Login']);
        return false;
    }
}
