import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, firstValueFrom, of, tap } from 'rxjs';
import { CabinetUserInfoModel } from '../../models/CabinetModel/cabinet-user-info.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { RegisterModel } from '../../models/Users/register-model';
import { buildApiUrl } from '../config/api.config';

@Injectable({ providedIn: 'root' })
export class UserService {
  private currentUserSubject = new BehaviorSubject<CabinetUserInfoModel | null>(null);

  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  get currentUserSnapshot(): CabinetUserInfoModel | null {
    return this.currentUserSubject.value;
  }

  get isAuthenticated(): boolean {
    return this.currentUserSnapshot !== null;
  }

  async register(data: RegisterModel): Promise<boolean> {
    const sendUrl = buildApiUrl('/Users/register');

    const res = await fetch(sendUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify(data)
    });

    if (!res.ok) {
      console.error('Register request failed:', await res.text());
      this.currentUserSubject.next(null);
      return false;
    }

    await this.loadCurrentUser();
    return true;
  }

  async login(email: string, password: string): Promise<boolean> {
    const sendUrl = buildApiUrl('/Users/login');

    const res = await fetch(sendUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      credentials: 'include',
      body: JSON.stringify({ email, password })
    });

    if (!res.ok) {
      this.currentUserSubject.next(null);
      return false;
    }

    await this.loadCurrentUser();
    return true;
  }

  restoreSession(): Observable<CabinetUserInfoModel | null> {
    return this.fetchCurrentUser().pipe(
      tap(user => this.currentUserSubject.next(user)),
      catchError(() => {
        this.currentUserSubject.next(null);
        return of(null);
      })
    );
  }

  Me(): Observable<CabinetUserInfoModel> {
    return this.fetchCurrentUser();
  }

  logout(): void {
    const url = buildApiUrl('/Users/logout');
    this.http.post(url, {}, { withCredentials: true }).subscribe({
      next: () => this.clearClientState(),
      error: () => this.clearClientState()
    });
  }

  private fetchCurrentUser(): Observable<CabinetUserInfoModel> {
    const url = buildApiUrl('/Users/me');
    return this.http.get<CabinetUserInfoModel>(url, { withCredentials: true });
  }

  private async loadCurrentUser(): Promise<void> {
    try {
      const user = await firstValueFrom(this.fetchCurrentUser());
      this.currentUserSubject.next(user);
    } catch {
      this.currentUserSubject.next(null);
    }
  }

  private clearClientState(): void {
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  clearAuthState(): void {
    this.currentUserSubject.next(null);
  }
}
