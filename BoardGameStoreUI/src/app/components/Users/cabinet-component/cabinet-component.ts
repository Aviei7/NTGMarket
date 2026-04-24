import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AvatarModule } from 'primeng/avatar';
import { PanelMenuModule } from 'primeng/panelmenu';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { CabinetService } from '../../../Core/services/cabinet.service';
import { UserService } from '../../../Core/services/user.service';
import { MenuItem } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { CabinetUserInfoModel } from '../../../models/CabinetModel/cabinet-user-info.model';

interface Order {
  id: string;
  date: string;
  total: number;
  status: string;
}

@Component({
  selector: 'app-cabinet-component',
  standalone: true,
  imports: [CommonModule, AvatarModule, PanelMenuModule, ButtonModule, TableModule, RouterModule],
  templateUrl: './cabinet-component.html',
  styleUrl: './cabinet-component.css'
})
export class CabinetComponent {
  userName: string = '';
  emailUser: string = '';
  lastName: string = '';
  private userSubscription?: Subscription;

  menuItems: MenuItem[] = [
    { label: 'Замовлення', icon: 'pi pi-list', routerLink: ['/cabinet/orders'] },
    { label: 'Кошик', icon: 'pi pi-shopping-cart', routerLink: ['/cart'] },
    { label: 'Персональні пропозиції', icon: 'pi pi-bell', badge: '1', routerLink: ['/cabinet/offers'] },
    // { label: 'Листування', icon: 'pi pi-comments', routerLink: ['/cabinet/messages'] },
    { label: 'Вихід з акаунту', icon: 'pi pi-sign-out', command: () => this.logout()}
  ];

 orders: Order[] = [
    { id: 'BG-1001', date: '2026-02-15', total: 75.50, status: 'Доставлено' },
    { id: 'BG-1002', date: '2026-02-20', total: 120.00, status: 'У процесі' },
    { id: 'BG-1003', date: '2026-02-25', total: 45.99, status: 'Скасовано' }
  ];

  activeTab = 0;

  constructor(private cabinetService: CabinetService, private userService: UserService) {}

  ngOnInit(): void {
    this.userSubscription = this.userService.currentUser$.subscribe((info: CabinetUserInfoModel | null) => {
      this.userName = info?.name ?? '';
      this.lastName = info?.lastName ?? '';
      this.emailUser = info?.email ?? '';
    });

    if (!this.userService.currentUserSnapshot) {
      this.userService.restoreSession().subscribe();
    }
  }

  ngOnDestroy(): void {
    this.userSubscription?.unsubscribe();
  }

  logout(): void {
    this.userService.logout();
    this.userName = '';
    this.emailUser = '';
    this.lastName = '';
  }

  selectTab(index: number): void {
    this.activeTab = index;
  }
}
