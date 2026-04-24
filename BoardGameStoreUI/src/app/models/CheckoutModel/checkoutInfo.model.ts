import { DeliveryMethod } from "./deliveryMethods";
import { PaymentMethod } from "./paymentMethods";

export interface CheckoutInfoModel {
   paymentMethods: PaymentMethod[];
   deliveryMethods: DeliveryMethod[];
}
