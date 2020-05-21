import { Observable } from "rxjs";
import { IUser } from "../models/User";
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root',
})

export class UserService {
  url: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(public http: HttpClient, private router: Router) {
    this.url = "https://localhost:44382/" + "api/Users/";
  }

  createUser(user: IUser): Observable<IUser> {
    return this.http.post<IUser>(this.url, user, this.httpOptions);
  }

  authorize(user: IUser) {
    this.getUserWithRole(user).subscribe(data => {
      sessionStorage.setItem("user", user.username);
      sessionStorage.setItem(user.username, data.role);
      this.router.navigate(['/rooms']);
    });
  }

  getUserWithRole(user: IUser): Observable<IUser> {
    return this.http.post<IUser>(this.url + "getrole/", user, this.httpOptions);
  }

  tryLogin(user: IUser): Observable<HttpResponse<any>> {
    let headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    return this.http.post<IUser>(this.url + "trylogin/", user, { observe: 'response', headers: headers });
  }
}
