import { Component, DestroyRef, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { UserService } from './Core/services/user.service';
import { BackendStatusService } from './Core/services/global.services/backend-status.service';
import { AppUnavailableComponent } from './components/app-unavailable-component/app-unavailable-component';
import { CartServices } from './Core/services/cart.services';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, ButtonModule, InputTextModule, FormsModule, CardModule, AppUnavailableComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private readonly backendStatusService = inject(BackendStatusService);
  private readonly cartServices = inject(CartServices);
  private readonly destroyRef = inject(DestroyRef);

  public readonly IsBackendAvailable = this.backendStatusService.IsBackendAvailable;

  isDark = false;
  inputSearchText: string = '';
  cartQuantity = 0;

  constructor(
    private router: Router,
    private userService: UserService) {}

  toggleDarkMode(): void {
    this.isDark = !this.isDark;
    if (this.isDark) {
      document.documentElement.classList.add('app-dark');
      localStorage.setItem('darkMode', 'true');
    } else {
      document.documentElement.classList.remove('app-dark');
      localStorage.setItem('darkMode', 'false');
    }
  }

  login(): void {
    alert('РћС‚РєСЂС‹С‚СЊ РѕРєРЅРѕ Р»РѕРіРёРЅР°');
  }

  onSearch(): void {
    if (this.inputSearchText && this.inputSearchText.trim()) {
      this.router.navigate(['/catalog'], { queryParams: { q: this.inputSearchText.trim() } });
    }
  }

  ngOnInit(): void {
    this.backendStatusService.StartMonitoring();
    this.userService.restoreSession().subscribe();
    this.cartServices.cartQuantity$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((quantity) => {
        this.cartQuantity = quantity;
      });

    this.cartServices.GetCart().subscribe();

    const saved = localStorage.getItem('darkMode');
    if (saved === 'true') {
      this.isDark = true;
      document.documentElement.classList.add('app-dark');
    }
  }
}
