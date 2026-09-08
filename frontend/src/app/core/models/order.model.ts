export type OrderStatus =
  | 'pending'
  | 'confirmed'
  | 'shipped'
  | 'delivered'
  | 'cancelled';


export interface Order {

  id: string;

  orderNumber: string;

  productId: string;

  quantity: number;

  total: number;

  status: OrderStatus;

  createdAt: string;

}