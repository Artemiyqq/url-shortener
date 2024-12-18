import { CommonModule } from '@angular/common';
import { Component, Input, input, OnInit } from '@angular/core';

@Component({
  selector: 'app-error-toast',
  imports: [CommonModule],
  templateUrl: './error-toast.component.html',
  standalone: true
})
export class ErrorToastComponent implements OnInit {
  @Input() message: string = '';

  isVisible: boolean = false;

  ngOnInit(): void {
    this.show();
  }

  show(): void {
    this.isVisible = true;
  }

  close(): void {
    this.isVisible = false;
  }
}
