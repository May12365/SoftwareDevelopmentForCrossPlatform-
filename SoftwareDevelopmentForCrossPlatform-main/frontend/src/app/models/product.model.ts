export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
}
export interface RepairRequest {
  request_id: number;
  request_no: string;
  title: string;
  decription: string;
  location: string;
  category: string;
  status: string;
  requester_id: number;
  approver_id: number;
  technician_id: number;
}