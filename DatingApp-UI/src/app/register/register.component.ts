import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { AuthService } from "../_services/auth.service";
import { AlertifyjsService } from '../_services/alertifyjs.service';

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.css"]
})
export class RegisterComponent implements OnInit {
  model: any = {};
 
  @Output() cancelRegister = new EventEmitter();
  constructor(private authService: AuthService, private alertify:AlertifyjsService) {}

  ngOnInit() {}

  register() {
    this.authService.register(this.model).subscribe(() => {
     this.alertify.success("Register Successfully");
    },err =>{
      this.alertify.error(err);
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
