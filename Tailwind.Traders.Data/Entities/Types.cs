using System;

namespace Tailwind.Traders.Data.Entities
{
    public class Types
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Name}";
        }
    }
}
