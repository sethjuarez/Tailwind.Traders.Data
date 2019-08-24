using System;
using System.Collections.Generic;
using System.Text;

namespace Tailwind.Traders.Data
{
    public class Company
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
    }
    public class Person
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public Company Company { get; set; }
    }

    public class Invoice
    {
        public int InvoiceNumber { get; set; }
        public Person Customer { get; set; }
        public List<LineItem> LineItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
    
    public class LineItem
    {
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal LineTotal { get; set; }
    }
}
