using System;

namespace Tailwind.Traders.Data
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
