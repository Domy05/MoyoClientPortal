import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  template: '<p>Completing sign in...</p>',
})
export class AuthCallback {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  constructor() {
    this.completeSignIn();
  }

  private async completeSignIn(): Promise<void> {
    try {
      await this.authService.handleOidcCallback();
      await this.router.navigate(['/dashboard']);
    } catch {
      await this.router.navigate(['/login']);
    }
  }
}
