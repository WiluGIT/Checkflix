import { Component, OnInit } from '@angular/core';
import { ThemePalette, ProgressSpinnerMode } from '@angular/material';

@Component({
  selector: 'app-content-spinner',
  templateUrl: './content-spinner.component.html',
  styleUrls: ['./content-spinner.component.css']
})
export class ContentSpinnerComponent {
  color: ThemePalette = 'primary';
  mode: ProgressSpinnerMode = 'indeterminate';
  constructor() { }

  ngOnInit() {
  }

}
