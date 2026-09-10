import { Component, inject } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
  NavigationEnd
} from '@angular/router';
import { filter } from 'rxjs/operators';

import { AuthService } from '../../core/services/auth.service';
import { SearchService } from '../../core/services/search.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {

  authService = inject(AuthService);
  private router = inject(Router);
  searchService = inject(SearchService);

  currentPage: 'products' | 'orders' | 'disabled' = 'disabled';

  constructor() {
    this.updateSearchState(this.router.url);

    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(event => {
        const navigation = event as NavigationEnd;

        this.updateSearchState(navigation.urlAfterRedirects);
      });
  }

  get client() {
    return this.authService.getClient();
  }

  get initials(): string {
    const client = this.client;

    if (!client) {
      return '';
    }

    const firstInitial = client.firstName?.charAt(0) ?? '';
    const lastInitial = client.lastName?.charAt(0) ?? '';

    return `${firstInitial}${lastInitial}`.toUpperCase();
  }

  get fullName(): string {
    const client = this.client;

    if (!client) {
      return '';
    }

    return `${client.firstName} ${client.lastName}`;
  }

  get searchEnabled(): boolean {
    return this.currentPage !== 'disabled';
  }

  get searchPlaceholder(): string {
    if (this.currentPage === 'products') {
      return 'Search products...';
    }

    if (this.currentPage === 'orders') {
      return 'Search orders...';
    }

    return 'Search unavailable';
  }

updateSearchState(url: string): void {

  if (url.startsWith('/products')) {

    this.currentPage = 'products';

    this.searchService.clearSearch();

    return;

  }

  if (url.startsWith('/orders')) {

    this.currentPage = 'orders';

    this.searchService.clearSearch();

    return;

  }

  this.currentPage = 'disabled';

  this.searchService.clearSearch();

}

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.searchService.setSearchTerm(input.value);
  }

  onAuthClick(): void {
    if (this.authService.isLoggedIn()) {
      this.authService.logout();
      this.router.navigate(['/login']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}