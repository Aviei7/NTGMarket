import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';
import { buildApiUrl } from '../../config/api.config';

@Injectable({
  providedIn: 'root'
})
export class BackendStatusService {
  private readonly http = inject(HttpClient);
  private monitoringStarted = false;

  public readonly IsBackendAvailable = signal(false);
  public readonly IsChecking = signal(false);

  public CheckBackend() {
    this.IsChecking.set(true);

    this.http.get(buildApiUrl('/Server/status'), { responseType: 'text' }).pipe(
      tap(() => {
        this.IsBackendAvailable.set(true);
        this.IsChecking.set(false);
      }),
      catchError(() => {
        this.IsBackendAvailable.set(false);
        this.IsChecking.set(false);
        return of(null);
      })
    ).subscribe();
  }

  public StartMonitoring() {
    if (this.monitoringStarted) {
      return;
    }

    this.monitoringStarted = true;
    this.CheckBackend();

    setInterval(() => {
      this.CheckBackend();
    }, 15000);
  }
}
