export interface Product {
  id: string;
  name: string;
  price: number;
  stock: number;
  category: string;
  image: string;
  images?: string[];
  description?: string;
  brand?: string;
  packSize?: string;
  weight?: string;
  unit?: string;
}