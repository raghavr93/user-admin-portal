import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from './auth.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
   constructor(private _authService: AuthService,
          private _router: Router){}

    canActivate(
      next: ActivatedRouteSnapshot
    ): boolean {
      if(this._authService.loggedIn()){
        let role = next.data['permitterRole'] as String
        if(role){
          if(this._authService.roleMatch(role)) return true;
          else{
            this._router.navigate(['/forbidden']);
            return false;
          }
        }
          return true;
      }else{
          this._router.navigate(['/admin'])
          return false;
      } 
    }
}
