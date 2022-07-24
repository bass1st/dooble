// Unlike components, a service is a singleton - the data that it stores is available through the lifetime of an application
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // Create an observable to store an user
  // ReplaySubject is kind of an "object buffer" observable, we define how many recent value(s) of an object we want to store
  // In this case, the size of a buffer is (1), hence one and only value is stored
  private currentUserSource = new ReplaySubject<User>(1);
  // For observable type variables, convention is to append the variable name with the "$" sign
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private presence: PresenceService) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getInfoFromToken(user.token).role;
    // Return type may be a string (single role) or string[] (user with multiple roles)
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.presence.stopHubConnection();
  }

  // Token = [header, payload, signature]. Get user info from token payload (only signature part is encrypted)
  getInfoFromToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
