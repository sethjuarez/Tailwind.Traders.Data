using System;

namespace Tailwind.Traders.Data.Entities
{
    public class Features
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProductItemId { get; set; }

        public override string ToString()
        {
            return $"{Title}, {Description}";
        }
    }
}
