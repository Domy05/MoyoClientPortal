import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { Dashboard } from './dashboard';

describe('Dashboard', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dashboard],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(Dashboard);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should load recent orders and compute stats on init', fakeAsync(() => {
    const fixture = TestBed.createComponent(Dashboard);
    const component = fixture.componentInstance;
    fixture.detectChanges();

    tick(300);

    expect(component.recentOrders.length).toBeGreaterThan(0);
    expect(component.totalOrders).toBeGreaterThan(0);
  }));
});