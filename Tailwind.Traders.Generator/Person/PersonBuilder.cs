using DataWorx.Core;
using System.Reflection;
using DataWorx.Core.Graph;
using DataWorx.Core.ObjectBuilders;

namespace Tailwind.Traders.Generator.Person
{
    public class PersonBuilder : ObjectBuilder
    {
        public PersonBuilder(Node node)
            : base(node)
        {

        }

        public override object Map(object o)
        {
            var a = PersonEntity.Random();
            foreach (PropertyInfo pi in typeof(PersonEntity).GetProperties())
                if (Node.Type.GetProperty(pi.Name, pi.PropertyType) != null)
                    Ject.Set(o, pi.Name, pi.GetValue(a));

            if (Next != null)
                return Next.Map(o);
            else
                return o;
        }
    }
}
