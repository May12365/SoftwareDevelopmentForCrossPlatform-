import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RepairRequest } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  // ชี้ URL ไปที่ Controller ฝั่งคำขอแจ้งซ่อม (ตามที่คุณตั้งค่า proxy ไว้สไตล์เดียวกับ /api/products)
  private apiUrl = '/api/requests';

  constructor(private http: HttpClient) {}

  getAll(): Observable<RepairRequest[]> {
    return this.http.get<RepairRequest[]>(this.apiUrl);
  }

  getById(id: number): Observable<RepairRequest> {
    return this.http.get<RepairRequest>(`${this.apiUrl}/${id}`);
  }

  create(request: Partial<RepairRequest>): Observable<RepairRequest> {
    return this.http.post<RepairRequest>(this.apiUrl, request);
  }

  update(id: number, request: Partial<RepairRequest>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}