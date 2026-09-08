import { Injectable } from '@angular/core';
import { of, delay } from 'rxjs';
import { Product } from '../models/product.model';

const MOCK_PRODUCTS: Product[] = [
  {
    id: '1',
    name: 'Coloured Pencil Set',
    price: 12,
    stock: 120,
    category: 'Stationery',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Stationery',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Stationery+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Stationery+2',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Stationery+3',
    ],
    description: 'A vibrant 24-colour pencil set, perfect for office notes, sketching and presentations.',
    brand: 'Moyo',
    packSize: '24 pencils',
    weight: '150g',
    unit: 'pack',
  },
  {
    id: '2',
    name: 'A4 Copy Paper',
    price: 89.99,
    stock: 243,
    category: 'Paper',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Paper',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Paper+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Paper+2',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Paper+3',
    ],
    description: 'High quality A4 copy paper, 80gsm. Perfect for everyday printing, copying and office use.',
    brand: 'Moyo',
    packSize: '500 sheets',
    weight: '80gsm',
    unit: 'pack',
  },
  {
    id: '3',
    name: 'Laser Toner Cartridge',
    price: 120,
    stock: 25,
    category: 'Printing',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Printing',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Printing+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Printing+2',
    ],
    description: 'High-yield laser toner cartridge, compatible with most standard office printers.',
    brand: 'Moyo',
    packSize: '1 cartridge',
    weight: '600g',
    unit: 'unit',
  },
  {
    id: '4',
    name: 'Lever Arch File',
    price: 45,
    stock: 80,
    category: 'Office',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Office',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Office+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Office+2',
    ],
    description: 'Durable A4 lever arch file with reinforced spine, ideal for long-term document storage.',
    brand: 'Moyo',
    packSize: '1 file',
    weight: '450g',
    unit: 'unit',
  },
  {
    id: '5',
    name: 'Surface Cleaner Spray',
    price: 18,
    stock: 40,
    category: 'Cleaning',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Cleaning',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Cleaning+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Cleaning+2',
    ],
    description: 'Multi-surface cleaning spray, safe for desks, glass and shared office equipment.',
    brand: 'Moyo',
    packSize: '750ml bottle',
    weight: '800g',
    unit: 'bottle',
  },
  {
    id: '6',
    name: 'Instant Coffee Jar',
    price: 25,
    stock: 55,
    category: 'Pantry',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Pantry',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Pantry+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Pantry+2',
    ],
    description: 'Rich instant coffee, perfect for stocking the office kitchen.',
    brand: 'Moyo',
    packSize: '200g jar',
    weight: '220g',
    unit: 'jar',
  },
  {
    id: '7',
    name: 'Ballpoint Pens (Box of 12)',
    price: 15,
    stock: 200,
    category: 'Stationery',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Stationery',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Pens+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Pens+2',
    ],
    description: 'Smooth-writing ballpoint pens in classic blue ink, boxed in a pack of 12.',
    brand: 'Moyo',
    packSize: '12 pens',
    weight: '90g',
    unit: 'box',
  },
  {
    id: '8',
    name: 'Sticky Notes Pack',
    price: 22,
    stock: 90,
    category: 'Paper',
    image: 'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Paper',
    images: [
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Notes+1',
      'https://placehold.co/500x400/e8e2d0/2f4f3f?text=Notes+2',
    ],
    description: 'Assorted sticky notes for reminders, labelling and quick office communication.',
    brand: 'Moyo',
    packSize: '6 pads',
    weight: '250g',
    unit: 'pack',
  },
];

@Injectable({ providedIn: 'root' })
export class ProductService {
  getProducts() {
    return of(MOCK_PRODUCTS).pipe(delay(300));
  }

  getProduct(id: string) {
    return of(MOCK_PRODUCTS.find((p) => p.id === id)).pipe(delay(300));
  }
}