import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/UserService';
import { IUser } from '../models/User';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  constructor(public userService:UserService) { }

  ngOnInit() {
  }

  addUser(username: string, password: string) {
    var user: IUser = { username, password } as IUser;
    this.userService
      .createUser(user)
      .subscribe(data => {
        this.userService.authorize(user);
      },
        Error => { alert("ошибка при добавлении нового пользователя") });

  }
}
