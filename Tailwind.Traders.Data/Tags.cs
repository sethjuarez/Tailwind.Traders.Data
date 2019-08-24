using System;

namespace Tailwind.Traders.Data
{
    public class Tags
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
