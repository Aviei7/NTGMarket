import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product-component',
  standalone: true,
  imports: [],
  templateUrl: './product-component.html',
  styleUrl: './product-component.css'
})
export class ProductComponent {

  productId!: number;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
  }
  
}
