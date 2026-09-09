export interface OrderItem {
  id: string;
  productId: string;
  quantity: number;
  unitPrice: number;
}

export interface Order {
  id: string;
  status: 'pending' | 'confirmed' | 'shipped' | 'cancelled';
  createdAt: string;
  orderItems: OrderItem[];
}