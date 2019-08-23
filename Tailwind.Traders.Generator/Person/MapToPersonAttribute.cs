using System;
using DataWorx.Core.Graph;
using DataWorx.Core.Attributes;
using DataWorx.Core.ObjectBuilders;

namespace Tailwind.Traders.Generator.Person
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MapToPersonAttribute : MapAttribute
    {
        public override IObjectBuilder CreateBuilder(Node node)
        {
            return new PersonBuilder(node);
        }
    }
}
