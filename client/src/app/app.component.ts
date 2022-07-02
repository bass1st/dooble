import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'dooble';
  users: any;
  // Dependency injection using a contructor, similar to .NET
  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsers();  
  }

  getUsers() {
    // Returns a response of type Observable, subscribe to it in order to retrieve data
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => {
        this.users = response;
      },
      error: error => {
        console.log(error);
      }
    });
  }
}
