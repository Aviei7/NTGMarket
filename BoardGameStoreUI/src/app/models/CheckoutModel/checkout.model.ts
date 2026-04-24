export interface CheckoutOrderModel {
    City: string;
    Address: string;
    Region: string;
    PostalCode: string;
    DeliveryMethod: number;
    PaymentMethod: number;
    PaymentStatusId: number;
    TotalPrice: number;
    FirstName: string;
    LastName: string;
    Email: string;
    Phone: string;
    Comment: string;
}

export interface CheckoutValidationModel {
    Email: string;
    Phone: string;
}

export interface CheckoutValidationResultModel {
    isValid: boolean;
    errors: string[];
}

export interface CheckoutSubmitResultModel {
    orderId: number;
    status: string;
    message: string;
}
