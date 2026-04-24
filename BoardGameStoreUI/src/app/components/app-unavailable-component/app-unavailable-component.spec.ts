import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppUnavailableComponent } from './app-unavailable-component';

describe('AppUnavailableComponent', () => {
  let component: AppUnavailableComponent;
  let fixture: ComponentFixture<AppUnavailableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppUnavailableComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AppUnavailableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
