export interface Product {
  id: number;
  name: string;
  price: number;
  categoryId: number;
  categoryName: string | null;
  primaryImageUrl: string | null; 
  description: string | null; 

}
