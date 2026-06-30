import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Product } from '../models/product.model';
import { ProductService } from '../services/product.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  errorMessage = '';

  // ใช้สำหรับฟอร์มเพิ่มสินค้าใหม่
  newProduct: Partial<Product> = { name: '', description: '', price: 0, stock: 0 };

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (data) => (this.products = data),
      error: (err) => (this.errorMessage = 'ไม่สามารถโหลดข้อมูลได้: ' + err.message)
    });
  }

  addProduct(): void {
    if (!this.newProduct.name) return;

    this.productService.create(this.newProduct).subscribe({
      next: () => {
        this.newProduct = { name: '', description: '', price: 0, stock: 0 };
        this.loadProducts();
      },
      error: (err) => (this.errorMessage = 'เพิ่มสินค้าไม่สำเร็จ: ' + err.message)
    });
  }

  deleteProduct(id: number): void {
    this.productService.delete(id).subscribe({
      next: () => this.loadProducts(),
      error: (err) => (this.errorMessage = 'ลบสินค้าไม่สำเร็จ: ' + err.message)
    });
  }
}
