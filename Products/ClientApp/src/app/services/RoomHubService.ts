import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { productChecked } from "../json/product-checked";

@Injectable({
  providedIn: 'root'
})

export class RoomHubService {
  private hubConnection: signalR.HubConnection;
  constructor() {
  }

  public startConnection = (roomId: number, username: string) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`https://localhost:44382/products`)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Connection started');
        this.tryEnterRoom(roomId, username);
      });

    this.hubConnection.on("EnterRoom", (roomId: number, username: string) => {
      sessionStorage.setItem(roomId.toString(), username);
      document.getElementById("passwordfield").remove();
      document.getElementById("roomContent").removeAttribute("hidden");
      this.hubConnection.invoke("AddToGroup", roomId);
      this.refreshData(roomId);
    });

    this.hubConnection.on("EnterPassword", () => {

    });

    this.hubConnection.on("GroupMessage", (username: string, message: string) => {
      var messages = document.getElementById("messages") as HTMLTextAreaElement;
      messages.value += `${username}:${message}`;
      messages.value += "\n";
      var textarea = document.getElementById("newmessage") as HTMLTextAreaElement;
      textarea.value = "";
    });

    this.hubConnection.on("WrongPassword", () => {
      alert("Неверный пароль!");
    });

    this.hubConnection.on("AddNewProduct", (product: string) => {
      var list = document.getElementById("products") as HTMLUListElement;
      var newProducts = document.createElement("li");
      newProducts.innerText = `${product}`;
      newProducts.addEventListener("click", (event) => {
        newProducts.setAttribute("class", "transparent-text");
      });
      list.appendChild(newProducts);
    });

    this.hubConnection.on("Data", (jsonUsers: string, jsonMessages: string, jsonProducts: string) => {
      var users: Array<string> = JSON.parse(jsonUsers);
      var messages: Array<string> = JSON.parse(jsonMessages);
      var products: Array<productChecked> = JSON.parse(jsonProducts);

      var listOfUsers = document.getElementById("users") as HTMLUListElement;
      var listOfProducts = document.getElementById("products") as HTMLUListElement;

      var messagesTextarea = document.getElementById("messages") as HTMLTextAreaElement;
      messagesTextarea.value = "";

      while (listOfUsers.childNodes[0]) {
        listOfUsers.childNodes[0].parentNode.removeChild(listOfUsers.childNodes[0]);
      }

      while (listOfProducts.childNodes[0]) {
        listOfProducts.childNodes[0].parentNode.removeChild(listOfProducts.childNodes[0]);
      }

      users.forEach((value) => {
        var user=document.createElement("li");
        user.innerHTML = value;
        listOfUsers.appendChild(user);
      });

      messages.forEach((value) =>
      {
        messagesTextarea.value += `${value}\n`;
      });

      products.forEach((value) => {
        var product = document.createElement("li");
        product.innerHTML = value.Product.toString();
        if (value.Checked) {
          product.setAttribute("style", "color: rgba(0, 0, 0, 0.2);");
        }
        listOfProducts.appendChild(product);
        product.addEventListener("click", () => {
          product.setAttribute("style", "color: rgba(0, 0, 0, 0.2);");
          this.checkProduct(roomId, product.innerHTML);
        });
      });
    });
  }

  tryEnterRoom(roomId: number, username: string) {
    this.hubConnection.invoke("TryEnter", roomId, username);
  }

  tryEnterUsingPassword(roomId: number, username: string, password: string) {
    this.hubConnection.invoke("TryEnterUsingPassword", roomId, username, password);
  }

  sendToGroup(roomId: number, username: string, message: string) {
    this.hubConnection.invoke("SendMessageToGroup", roomId, username, message);
  }

  addNewProduct(roomId: number, product: string) {
    this.hubConnection.invoke("AddNewProduct", roomId, product);
  }

  refreshData(roomId:number) {
    this.hubConnection.invoke("GetData", roomId);
  }

  checkProduct(roomId:number, product:string) {
    this.hubConnection.invoke("CheckProduct", roomId, product);
    this.refreshData(roomId);
  }

  clearProducts(roomId:number) {
    this.hubConnection.invoke("ClearProducts", roomId);
    this.refreshData(roomId);
  }
}
