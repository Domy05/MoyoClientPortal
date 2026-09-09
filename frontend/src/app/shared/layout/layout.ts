import { Component, inject } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

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

  onAuthClick() {
    if (this.authService.isLoggedIn()) {
      this.authService.logout();
      this.router.navigate(['/login']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}