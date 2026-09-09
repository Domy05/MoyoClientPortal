import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, provideRouter } from '@angular/router';
import { ProductDetail } from './product-detail';

describe('ProductDetail', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProductDetail],
      providers: [
        provideRouter([]),
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: { paramMap: convertToParamMap({ id: '1' }) },
          },
        },
      ],
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(ProductDetail);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });

  it('should load the product matching the route id', async () => {
    const fixture = TestBed.createComponent(ProductDetail);
    const component = fixture.componentInstance;
    fixture.detectChanges();

    await new Promise((resolve) => setTimeout(resolve, 350));

    expect(component.product()?.id).toBe('1');
  });
});