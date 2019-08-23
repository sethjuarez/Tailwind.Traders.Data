using System;
using System.Linq;
using System.Collections.Generic;
using DataWorx.Core.Graph;
using DataWorx.Core.Math;
using System.Reflection;
using DataWorx.Core.ObjectBuilders;
using DataWorx.Core;
using System.IO;
using CsvHelper;
using DataWorx.Core.Attributes;

namespace Tailwind.Traders.Generator.Address
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MapToAddressAttribute : MapAttribute
    {
        public override IObjectBuilder CreateBuilder(Node node)
        {
            return new AddressBuilder(node);
        }
    }
}
