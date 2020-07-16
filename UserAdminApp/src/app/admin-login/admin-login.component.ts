import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-admin-login',
  templateUrl: './admin-login.component.html',
  styleUrls: ['./admin-login.component.css']
})
export class AdminLoginComponent implements OnInit {

  loginForm:FormGroup;
  showErrorMessage:boolean = false;
  constructor(private fb: FormBuilder,private _auth: AuthService,private _router: Router) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['',[Validators.required,Validators.email]],
      password: ['',Validators.required]
    }); 
  }

  get email() {
    return this.loginForm.get('email');
  }
  
  get password() {
    return this.loginForm.get('password');
  }

  onSubmit() {
    console.log(this.loginForm.value);
    this._auth.loginAdmin(this.loginForm.value)
      .subscribe(
        res =>{
          localStorage.setItem('token',res.token),
          this._router.navigate(['/user'])
          console.log(res)},
      err => {
        this.showErrorMessage = true;
        console.log(err)
      });
  }

}
