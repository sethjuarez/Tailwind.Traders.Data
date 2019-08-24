using DataWorx.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Tailwind.Traders.Generator.Address;
using Tailwind.Traders.Generator.Persons;

namespace Tailwind.Traders.Generator
{
    [MapToPerson, MapToAddress, Count(2000, 3000)]
    public class PersonGen
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
        
        [PhoneField]
        public string Phone { get; set; }
        
        public string Email { get; set; }
    }

    [MapToAddress, Count(100, 200)]
    public class CompanyGen
    {
        [NameField(5, 9)]
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
}
