import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(private router: Router) { }
  username: string;
  role: string;
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  getUser() {
    return sessionStorage.getItem("user");
  }

  getRole() {
    return sessionStorage.getItem(sessionStorage.getItem("user"));
  }

  logout() {
    sessionStorage.clear();
    this.router.navigate(['']);
  }
}
