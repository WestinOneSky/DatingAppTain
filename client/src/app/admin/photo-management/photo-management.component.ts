import { Component, OnInit } from '@angular/core';
import { find, findIndex } from 'rxjs/operators';
import { Photo } from 'src/app/_models/photo';

import { AdminService } from 'src/app/_services/admin.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[];


  constructor(private adminService: AdminService, private memberService: MembersService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotoForApproval().subscribe((photo: Photo[]) => {
      this.photos = photo;
    })
  }

  approvePhoto(id: number) {
    this.adminService.approvePhoto(id).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === id), 1)
    })
    console.log("approve");
  }

  disapprovePhoto(id: number) {
    this.adminService.disapprovePhoto(id).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === id), 1)
    })
    console.log("approve");
  }

}
