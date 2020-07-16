import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  users: any;
  totalRecords: string
  page:Number = 1
  constructor(private _router:Router,public _authService:AuthService) { }

  ngOnInit(): void {
    this.getAllUsers();
  }

  getAllUsers(){
    this._authService.getUsers().subscribe(
      res => {this.users = res
        this.totalRecords = res.length
        console.log(this.totalRecords);
      console.log(res)},
      error => console.error('Error!', error)
    );
  }

  isAdmin(){
    return this._authService.roleMatch('Admin') 
  }

}
