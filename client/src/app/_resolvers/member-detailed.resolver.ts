import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";
import { Observable } from "rxjs";

import { Member } from "../_models/member";
import { MembersService } from "../_services/members.service";

@Injectable({ providedIn: 'root' })
export class MemberDetailedResolver implements Resolve<Member> {
    constructor(private membersService: MembersService) {}
    // Router manages subscribe / unsubscribe automatically
    resolve(route: ActivatedRouteSnapshot): Observable<Member> {
        return this.membersService.getMember(route.paramMap.get('username'));
    }
}
