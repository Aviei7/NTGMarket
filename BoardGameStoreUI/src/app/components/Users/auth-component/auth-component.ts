import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { CardModule } from 'primeng/card';

/*Services*/
import { UserService } from '../../../Core/services/user.service';
/*Services*/

@Component({
  selector: 'app-auth-component',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    CardModule
  ],
  templateUrl: './auth-component.html',
  styleUrl: './auth-component.css'
})
export class AuthComponent {
  email: string = '';
  password: string = '';

  errorMessage: string = '';

  constructor(private userService: UserService, private router: Router) {}

  ngOnInit(): void {
    this.clearError();
  }

  private inAuth(){
    this.router.navigate(['cabinet']);
  }

  private setError(msg: string) {
    this.errorMessage = msg;
  }

  private clearError() {
    this.errorMessage = '';
  }


  
  async onLogin() {
    this.clearError();
    const success = await this.userService.login(this.email, this.password);
    if (success) {
      console.log('Login successful');
      this.inAuth();
    } else {
      this.setError('Не вдалося увійти. Перевірте свої облікові дані та спробуйте ще раз.');
    }
  }

  onSignUp() {
    this.router.navigate(['register']);
  }
}
