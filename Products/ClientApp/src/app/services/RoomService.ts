import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { IRoom } from "../models/Room";

@Injectable({
  providedIn: 'root',
})

export class RoomService {
  url: string;
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  constructor(public http: HttpClient, public router: Router) {
    this.url = "https://localhost:44382/" + "api/Rooms/";
  }

  createRoom(room: IRoom): Observable<IRoom> {
    return this.http.post<IRoom>(this.url, room, this.httpOptions);
  }

  getRooms(username: string): Observable<IRoom[]> {
    let params = { username: username };
    return this.http.get<IRoom[]>(this.url + "getrooms", {params:params});
  }
}
