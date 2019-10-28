import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { fadeInOut } from 'src/app/services/animations';

@Component({
    selector: 'not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.css'],
    animations: [fadeInOut]
})
export class NotFoundComponent
{
  constructor(private _location: Location) { }

  goBack() {
    this._location.back();
  }
}
