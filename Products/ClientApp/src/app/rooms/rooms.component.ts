import { Component, OnInit } from '@angular/core';
import { IRoom } from '../models/Room';
import { RoomService } from '../services/RoomService';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit {
  isCreatingRoom: boolean;
  rooms: Observable<Array<IRoom>>;
  constructor(public roomService:RoomService, public router:Router) { }

  ngOnInit() {
    this.rooms = this.roomService.getRooms(sessionStorage.getItem("user"));
  }

  ngAfterViewInit() {

  }

  createRoom(name: string, password: string) {
    var creator = sessionStorage.getItem("user");
    var room: IRoom = { name, password, creator } as IRoom;
    this.roomService.createRoom(room).subscribe(x => {
      this.router.navigate([`/room/${x.id}`]);
    }), error => {
      alert("что-то пошло не так!")
    };
  }
}
