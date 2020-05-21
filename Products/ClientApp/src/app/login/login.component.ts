import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { IUser } from '../models/User';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {

  constructor(public userService: UserService) { }

  ngOnInit() {
  }

  tryLogin(username: string, password: string) {
    var user: IUser = { username, password } as IUser;
    this.userService
      .tryLogin(user).subscribe(ok => this.userService.authorize(user), err => alert("неверный логин или пароль"));
  }

}
