using DataWorx.Core;
using DataWorx.Core.Graph;
using DataWorx.Core.ObjectBuilders;
using System;
using System.Linq;
using System.Reflection;

namespace Tailwind.Traders.Generator.Address
{
    public class AddressBuilder : ObjectBuilder
    {
        public AddressBuilder(Node node)
            : base(node)
        {

        }

        public override object Map(object o)
        {
            var a = AddressEntity.Random();
            foreach (PropertyInfo pi in typeof(AddressEntity).GetProperties())
                if (Node.Type.GetProperty(pi.Name, pi.PropertyType) != null)
                    Ject.Set(o, pi.Name, pi.GetValue(a));

            if (Next != null)
                return Next.Map(o);
            else
                return o;
        }
    }
}
