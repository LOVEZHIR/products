import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RoomService } from '../services/RoomService';
import { RoomHubService } from '../services/RoomHubService';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {
  id: number;
  isAuth: boolean;
  username = () => sessionStorage.getItem("user");
  constructor(private route: ActivatedRoute, public roomService: RoomService, private router: Router, public roomHubService:RoomHubService) { }

  ngOnInit() {
    this.isAuth = false;
    this.id = +this.route.snapshot.paramMap.get('id');
    this.roomHubService.startConnection(this.id, this.username());
  }

  tryEnterUsingPassword(password:string) {
    this.roomHubService.tryEnterUsingPassword(this.id, this.username(), password);
  }

  sendMessage() {
    var textarea = document.getElementById("newmessage") as HTMLTextAreaElement;
    if (textarea.value != "") {
      this.roomHubService.sendToGroup(this.id, this.username(), textarea.value);
      this.refreshData();
    } 
  }

  autoScroll() {
    var textarea = document.getElementById("messages") as HTMLTextAreaElement;
    textarea.scrollTop = textarea.scrollHeight;
  }

  ngAfterViewChecked() {
    this.autoScroll();
  }

  addProduct(product: string) {
    var productName = document.getElementById("productname") as HTMLInputElement;
    productName.value = "";
    if (product != "") {
      this.roomHubService.addNewProduct(this.id, product);
      this.refreshData();
    }
  }

  refreshData() {
    this.roomHubService.refreshData(this.id);
  }

  clearProducts() {
    this.roomHubService.clearProducts(this.id);
  }
}
