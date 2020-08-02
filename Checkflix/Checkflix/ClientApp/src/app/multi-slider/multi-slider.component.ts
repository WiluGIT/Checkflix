import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
import { NgModuleResolver } from '@angular/compiler';

@Component({
  selector: 'app-multi-slider',
  templateUrl: './multi-slider.component.html',
  styleUrls: ['./multi-slider.component.css']
})
export class MultiSliderComponent implements OnInit {
  @ViewChild('inputRight', { static: false }) inputRight;
  @ViewChild('inputLeft', { static: false }) inputLeft;
  @ViewChild('thumbLeft', { static: false }) thumbLeft;
  @ViewChild('thumbRight', { static: false }) thumbRight;
  @ViewChild('rangeBar', { static: false }) rangeBar;
  @Input() minValue:number;
  @Input() maxValue:number;

  leftValue: number;
  rightValue: number;
  constructor() {

  }

  ngOnInit() {
    this.leftValue = this.minValue;
    this.rightValue = this.maxValue;

  }

  mouseOver(e) {
    if (e.target.id === "input-left") {
      this.thumbLeft.nativeElement.classList.add("hover");
    } else if (e.target.id === "input-right") {
      this.thumbRight.nativeElement.classList.add("hover");
    }
  }
  mouseOut(e) {
    if (e.target.id === "input-left") {
      this.thumbLeft.nativeElement.classList.remove("hover");
    } else if (e.target.id === "input-right") {
      this.thumbRight.nativeElement.classList.remove("hover");
    }
  }
  mouseDown(e) {
    if (e.target.id === "input-left") {
      this.thumbLeft.nativeElement.classList.add("active");
    } else if (e.target.id === "input-right") {
      this.thumbRight.nativeElement.classList.add("active");
    }
  }
  mouseUp(e) {
    if (e.target.id === "input-left") {
      this.thumbLeft.nativeElement.classList.remove("active");
    } else if (e.target.id === "input-right") {
      this.thumbRight.nativeElement.classList.remove("active");
    }
  }

  setRightValue() {
    console.log("clicked r")

    let min = parseInt(this.inputRight.nativeElement.min);
    let max = parseInt(this.inputRight.nativeElement.max);
    
    this.inputRight.nativeElement.value = Math.max(parseInt(this.inputRight.nativeElement.value), parseInt(this.inputLeft.nativeElement.value) + 1);
    this.rightValue = this.inputRight.nativeElement.value;

    console.log(this.inputRight.nativeElement.value)

    var percent = ((this.inputRight.nativeElement.value - min) / (max - min)) * 100;

    this.thumbRight.nativeElement.style.right = (100 - percent) + "%";
    this.rangeBar.nativeElement.style.right = (100 - percent) + "%";
  }

  setLeftValue() {
    console.log("clicked l")

    let min = parseInt(this.inputLeft.nativeElement.min);
    let max = parseInt(this.inputLeft.nativeElement.max);
    

    this.inputLeft.nativeElement.value = Math.min(parseInt(this.inputLeft.nativeElement.value), parseInt(this.inputRight.nativeElement.value) - 1);
    this.leftValue = this.inputLeft.nativeElement.value;

    console.log(this.inputLeft.nativeElement.value)

    var percent = ((this.inputLeft.nativeElement.value - min) / (max - min)) * 100;

    this.thumbLeft.nativeElement.style.left = percent + "%";
    this.rangeBar.nativeElement.style.left = percent + "%";
  }
}
