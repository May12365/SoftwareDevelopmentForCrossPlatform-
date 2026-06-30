import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { ProductListComponent } from './product-list/product-list.component';
import { RequestListComponent } from './request-list/request-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ProductListComponent, RequestListComponent, CommonModule],
  // เปลี่ยนมาเปลี่ยนเส้นทางชี้ไปที่ไฟล์ภายนอกแทน
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'product-frontend';
  view: string = 'products'; // ตัวแปรควบคุมการเปลี่ยนหน้า
}