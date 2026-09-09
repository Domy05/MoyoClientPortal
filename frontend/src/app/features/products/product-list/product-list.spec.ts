import { TestBed } from '@angular/core/testing';
import { ProductList } from './product-list';

describe('ProductList', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductList],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(ProductList);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should load products on init', async () => {
    const fixture = TestBed.createComponent(ProductList);
    const component = fixture.componentInstance;
    fixture.detectChanges();

    await new Promise((resolve) => setTimeout(resolve, 350));

    expect(component.products().length).toBeGreaterThan(0);
  });

  it('should filter products by category when a pill is selected', async () => {
    const fixture = TestBed.createComponent(ProductList);
    const component = fixture.componentInstance;
    fixture.detectChanges();

    await new Promise((resolve) => setTimeout(resolve, 350));

    component.selectFilter('Paper');
    expect(component.filteredProducts().every((p) => p.category === 'Paper')).toBe(true);
  });
});