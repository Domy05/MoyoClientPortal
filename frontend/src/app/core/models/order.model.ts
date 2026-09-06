export interface Order {
  id: string;
  productId: string;
  quantity: number;
  status: 'pending' | 'confirmed' | 'shipped' | 'cancelled';
  createdAt: string;
}