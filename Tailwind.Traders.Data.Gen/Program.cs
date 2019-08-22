using System;

namespace Tailwind.Traders.Data.Gen
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new BaseDataGenerator();
            generator.Seed();
        }
    }
}
