using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDM.Payment.Address
{
    public class CheckoutAddressResponseModel
    {
        public class Address
        {
            public string GivenName { get; set; }
            public string FamilyName { get; set; }
            public string Email { get; set; }
            public string Title { get; set; }
            public string StreetAddress { get; set; }
            public string StreetAddress2 { get; set; }
            public string StreetName { get; set; }
            public string StreetNumber { get; set; }
            public string HouseExtension { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string Region { get; set; }
            public string Phone { get; set; }
            public string Country { get; set; }
            public string CareOf { get; set; }
        }

        public class Attachment
        {
            public string Body { get; set; }
            public string ContentType { get; set; }
        }

        public class DeliveryDetails
        {
            public string Carrier { get; set; }
            public Product Product { get; set; }
            public Timeslot Timeslot { get; set; }
            public PickupLocation PickupLocation { get; set; }
        }

        public class Dimensions
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public int Length { get; set; }
        }

        public class ExternalPaymentMethod
        {
            public string Name { get; set; }
            public int Fee { get; set; }
            public string Description { get; set; }
            public List<string> Countries { get; set; }
            public string Label { get; set; }
            public string RedirectUrl { get; set; }
            public string ImageUrl { get; set; }
        }

        public class OrderLine
        {
            public string Type { get; set; }
            public string Reference { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
            public Subscription Subscription { get; set; }
            public string QuantityUnit { get; set; }
            public int UnitPrice { get; set; }
            public int TaxRate { get; set; }
            public int TotalAmount { get; set; }
            public int TotalDiscountAmount { get; set; }
            public int TotalTaxAmount { get; set; }
            public string MerchantData { get; set; }
            public string ProductUrl { get; set; }
            public string ImageUrl { get; set; }
            public ProductIdentifiers ProductIdentifiers { get; set; }
            public ShippingAttributes ShippingAttributes { get; set; }
        }

        public class PickupLocation
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public Address Address { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }
            public string Identifier { get; set; }
        }

        public class ProductIdentifiers
        {
            public string Brand { get; set; }
            public string Color { get; set; }
            public string CategoryPath { get; set; }
            public string GlobalTradeItemNumber { get; set; }
            public string ManufacturerPartNumber { get; set; }
            public string Size { get; set; }
        }

        public class Root
        {
            public int OrderAmount { get; set; }
            public int OrderTaxAmount { get; set; }
            public string MerchantData { get; set; }
            public List<OrderLine> OrderLines { get; set; }
            public List<ShippingOption> ShippingOptions { get; set; }
            public Attachment Attachment { get; set; }
            public string PurchaseCurrency { get; set; }
            public string Locale { get; set; }
            public List<ExternalPaymentMethod> ExternalPaymentMethods { get; set; }
            public List<string> Tags { get; set; }
        }

        public class SelectedAddon
        {
            public string Type { get; set; }
            public int Price { get; set; }
            public string ExternalId { get; set; }
            public string UserInput { get; set; }
        }

        public class ShippingAttributes
        {
            public int Weight { get; set; }
            public Dimensions Dimensions { get; set; }
            public List<string> Tags { get; set; }
        }

        public class ShippingOption
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Promo { get; set; }
            public int Price { get; set; }
            public bool Preselected { get; set; }
            public int TaxAmount { get; set; }
            public int TaxRate { get; set; }
            public string ShippingMethod { get; set; }
            public DeliveryDetails DeliveryDetails { get; set; }
            public string TmsReference { get; set; }
            public List<SelectedAddon> SelectedAddons { get; set; }
        }

        public class Subscription
        {
            public string Name { get; set; }
            public string Interval { get; set; }
            public int IntervalCount { get; set; }
        }

        public class Timeslot
        {
            public string Id { get; set; }  
            public string Start { get; set; }
            public string End { get; set; }
        }


    }
}
