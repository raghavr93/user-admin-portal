import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})


export class AuthService {

  private _userUrl = "http://localhost:61078/api/user"
  constructor(private http:HttpClient, private _router: Router ) { 
  }

  getUsers():Observable<any>{
    return this.http.get<any>(this._userUrl)
  }
  checkUser(email){
    return this.http.post<any>(this._userUrl+"/exists",email);
  }
  registerUser(user):Observable<any>{
    return this.http.post<any>(this._userUrl+"/register",user)
  }

  loginUser(user):Observable<any>{
    return this.http.post<any>(this._userUrl+"/authenticate", user)
  }

  loginAdmin(user):Observable<any>{
    return this.http.post<any>(this._userUrl+"/admin-auth", user)
  }

  loggedIn(){
    return !!localStorage.getItem('token')
  }

  logoutUser(){
    localStorage.removeItem('token')
    this._router.navigate(['/login'])
  }

  getToken(){
    return localStorage.getItem('token')
  }

  roleMatch(allowedRole): boolean{
    var isMatch = false;
    var payload = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
    var userRole = payload.role;
    if(userRole == allowedRole)
    {
      isMatch = true;
    }
    return isMatch;
  }

}
