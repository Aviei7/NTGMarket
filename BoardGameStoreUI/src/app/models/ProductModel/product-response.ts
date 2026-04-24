import { Product } from "../ProductModel/product";

export interface ProductResponse {
    items: Product[];
    totalCount: number;
    page: number;
    pageSize: number;
    hasMoreItems: boolean;
    minPrice: number;
    maxPrice: number;
}
