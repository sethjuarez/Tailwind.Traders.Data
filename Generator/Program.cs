using System;
using Tailwind.Traders.Generator;
using Tailwind.Traders.Generator.Address;
using Tailwind.Traders.Generator.Person;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var products = Loader.LoadProductGraph();
            var addresses = AddressEntity.Load();
            var people = PersonEntity.Load();
        }
    }
}
