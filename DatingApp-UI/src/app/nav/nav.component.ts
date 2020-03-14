import { Component, OnInit } from "@angular/core";
import { AuthService } from '../_services/auth.service';
import { AlertifyjsService } from '../_services/alertifyjs.service';

@Component({
  selector: "app-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"]
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public authService:AuthService,private alertify:AlertifyjsService) {}

  ngOnInit(): void {}

  login() {
    debugger;
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success('Logged in Successfully');
    },err =>{
      this.alertify.error(err);
    });
  }

  loggedIn(){
    // const token = localStorage.getItem('token');
    // return !!token;
    return this.authService.loggedIn();
  }
  logOut(){
    localStorage.removeItem('token');
   this.alertify.message('Logged out');
  }
}
