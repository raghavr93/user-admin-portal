import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { FormBuilder, FormGroup, Validators, ValidatorFn, AsyncValidatorFn, AbstractControl, ValidationErrors, FormControl } from '@angular/forms';
import { map} from "rxjs/operators";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  datepickerConfig: Partial<BsDatepickerConfig>;
  registrationForm:FormGroup;
  constructor(private fb: FormBuilder,private _auth: AuthService,
              private _router: Router) { 
                this.datepickerConfig = Object.assign({}, 
                  { containerClass: 'theme-dark-blue',
                    minDate: new Date(1920,0,1),
                    maxDate: new Date(),  
                  })
              }

  ngOnInit() {
    this.registrationForm = this.fb.group({
      firstname: ['',[Validators.required,Validators.maxLength(100)]],
      email: ['',[Validators.required,Validators.email]],
      lastname: ['',Validators.maxLength(100)],
      dateOfBirth: ['', Validators.required],
      password: ['',[Validators.minLength(8),Validators.required]],
      address: ['',Validators.maxLength(300)],
      role: ['',Validators.required]
      });
  }
  
  get firstname() {
    return this.registrationForm.get('firstname');
  }
  get email() {
    return this.registrationForm.get('email');
  }
  get lastname() {
    return this.registrationForm.get('lastname');
  }
  get password() {
    return this.registrationForm.get('password');
  }
  get address(){
    return this.registrationForm.get('address');
  }
  get dateOfBirth(){
    return this.registrationForm.get('dateOfBirth');
  }
  get role(){
    return this.registrationForm.get('role');
  }
 
  validateEmailNotTaken(control: AbstractControl) {
    return this._auth.checkUser(control.value).pipe(
      map(res => {
        return res ? null : { emailTaken: true };
      })
    );
  }
    
  onSubmit(){
    console.log(this.registrationForm.value);
    this._auth.registerUser(this.registrationForm.value)
      .subscribe(
        res =>{
          this._router.navigate(['/user'])
          console.log(res)},
        error => console.error('Error!', error)
      );
  }


}

