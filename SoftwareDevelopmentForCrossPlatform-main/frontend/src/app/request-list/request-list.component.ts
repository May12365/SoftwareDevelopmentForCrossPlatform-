import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { RequestService } from '../services/request.service';
import { RepairRequest } from '../models/product.model';

@Component({
  selector: 'app-request-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css'] // ใช้หน้ากากตกแต่งชุดเดียวกับสินค้าได้เลย
})
export class RequestListComponent implements OnInit {
  repairRequests: RepairRequest[] = [];
  errorMessage: string = '';

  // โมเดลสำหรับผูกรับค่าข้อมูลเพิ่มใหม่จากหน้าฟอร์ม HTML
  newRequest: Partial<RepairRequest> = {
    request_no: '',
    title: '',
    decription: '',
    location: '',
    category: '',
    status: 'Pending',
    requester_id: 1,
    approver_id: 1,
    technician_id: 1,
  };

  constructor(private requestService: RequestService) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.requestService.getAll().subscribe({
      next: (data) => this.repairRequests = data,
      error: (err) => this.errorMessage = 'ไม่สามารถดึงข้อมูลรายการแจ้งซ่อมได้'
    });
  }

  addRequest(): void {
    this.requestService.create(this.newRequest).subscribe({
      next: () => {
        this.loadRequests();
        // ล้างค่าในฟอร์มเพื่อเคลียร์ข้อมูลเดิมออก
        this.newRequest = { request_no: '', title: '', decription: '', location: '', category: '', status: 'Pending', requester_id: 1, approver_id: 1, technician_id: 1 };
      },
      error: (err) => this.errorMessage = 'ไม่สามารถเพิ่มข้อมูลได้ โปรดลองอีกครั้ง'
    });
  }

  deleteRequest(id: number): void {
    if (confirm('คุณต้องการลบรายการแจ้งซ่อมนี้ใช่หรือไม่?')) {
      this.requestService.delete(id).subscribe({
        next: () => this.loadRequests(),
        error: (err) => this.errorMessage = 'ไม่สามารถลบข้อมูลได้'
      });
    }
  }
}