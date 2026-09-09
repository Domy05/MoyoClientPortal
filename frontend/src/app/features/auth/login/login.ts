import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private location = inject(Location);
  private authService = inject(AuthService);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
    rememberMe: [false],
  });

  goBack() {
    this.location.back();
  }

  submit() {
    if (this.form.invalid) {
      return;
    }

    const email = this.form.controls.email.value;
    const password = this.form.controls.password.value;

    if (!email || !password) {
      return;
    }

    this.authService.login({
      email: email,
      password: password
    }).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },

      error: () => {
        alert('Invalid email or password.');
      }
    });
  }
}