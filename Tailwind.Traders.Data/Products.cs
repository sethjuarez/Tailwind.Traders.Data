using System;
using System.Collections.Generic;

namespace Tailwind.Traders.Data
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageName { get; set; }
        public int BrandId { get; set; }
        public Brands Brand { get; set; }
        public int TypeId { get; set; }
        public Types Type { get; set; }
        public int TagId { get; set; }
        public Tags Tag { get; set; }

        public List<Features> Features { get; set; }

        public override string ToString()
        {
            return $"{Name}, ${Price}, {ImageName}";
        }
    }
}
