import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/auth/login/login').then(m => m.Login) },
  {
    path: '',
    loadComponent: () => import('./shared/layout/layout').then(m => m.Layout),
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard').then(m => m.Dashboard) },
      { path: 'products', loadComponent: () => import('./features/products/product-list/product-list').then(m => m.ProductList) },
      { path: 'orders', loadComponent: () => import('./features/orders/order-list/order-list').then(m => m.OrderList) },
      { path: 'orders/new', loadComponent: () => import('./features/orders/order-form/order-form').then(m => m.OrderForm) },
    ],
  },
  { path: '**', redirectTo: '' },
];