using System;
using Tailwind.Traders.Generator;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Generate generate = new Generate();
            generate.Create();
        }
    }
}
