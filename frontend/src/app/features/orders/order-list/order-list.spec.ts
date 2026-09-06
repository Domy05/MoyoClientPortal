import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { OrderList } from './order-list';

describe('OrderList', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrderList],
      providers: [provideRouter([])],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(OrderList);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});