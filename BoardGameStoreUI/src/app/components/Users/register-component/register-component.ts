import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { RegisterModel } from '../../../models/Users/register-model';
import { UserService } from '../../../Core/services/user.service';

@Component({
  selector: 'app-register-component',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register-component.html',
  styleUrl: './register-component.css'
})
export class RegisterComponent {
  model: RegisterModel = {
    firstName: '',
    lastName: '',
    phone: '',
    email: '',
    password: '',
    confirmPassword: ''
  };

  constructor(private userService: UserService, private route: Router) {}

  get passwordsMismatch(): boolean {
    return Boolean(this.model.password && this.model.confirmPassword)
      && this.model.password !== this.model.confirmPassword;
  }

  normalizePhone(): void {
    const digits = this.model.phone.replace(/\D/g, '');

    if (!digits) {
      this.model.phone = '';
      return;
    }

    if (digits.startsWith('380')) {
      this.model.phone = `+${digits}`;
      return;
    }

    if (digits.length === 10 && digits.startsWith('0')) {
      this.model.phone = `+38${digits}`;
      return;
    }

    if (digits.length === 9) {
      this.model.phone = `+380${digits}`;
    }
  }

  onSubmit(form: NgForm): void {
    this.normalizePhone();

    if (form.invalid || this.passwordsMismatch) {
      form.control.markAllAsTouched();
      return;
    }

    this.userService.register(this.model).then(success => {
      if (success) {
        console.log('Registration successful');
        this.route.navigate(['/cabinet']);
      } else {
        console.log('Registration failed');
      }
    });
  }
}
