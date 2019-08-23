using System;

namespace Tailwind.Traders.Data.Entities
{
    public class Brands
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
