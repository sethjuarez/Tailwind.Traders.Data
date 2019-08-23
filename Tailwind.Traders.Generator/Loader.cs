using CsvHelper;
using DataWorx.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Tailwind.Traders.Data.Entities;

namespace Tailwind.Traders.Generator
{
    public class Loader
    {
        public static List<Products> LoadProductGraph()
        {
            var brands = For<Brands>("Tailwind.Traders.Generator.Data.ProductBrands.csv").ToList();
            var features = For<Features>("Tailwind.Traders.Generator.Data.ProductFeatures.csv").ToList();
            var products = For<Products>("Tailwind.Traders.Generator.Data.ProductItems.csv").ToList();
            var tags = For<Tags>("Tailwind.Traders.Generator.Data.ProductTags.csv").ToList();
            var types = For<Types>("Tailwind.Traders.Generator.Data.ProductTypes.csv").ToList();

            // n-n-n-normalize!
            foreach (var p in products)
            {
                if (p.BrandId > 0)
                    p.Brand = brands.Where(b => b.Id == p.BrandId).First();
                if (p.TagId > 0)
                    p.Tag = tags.Where(t => t.Id == p.TagId).First();
                if (p.TypeId > 0)
                    p.Type = types.Where(t => t.Id == p.TypeId).First();
                
                p.Features = features.Where(f => f.ProductItemId == p.Id).ToList();
            }
            

            return products;
        }
        public static IEnumerable<T> For<T>(string path)
        {
            var assembly = typeof(Loader).Assembly;
            using (TextReader stream = new StreamReader(assembly.GetManifestResourceStream(path)))
            using (var csvReader = new CsvReader(stream))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                var headers = csvReader.Context.HeaderRecord;
                while (csvReader.Read())
                {
                    var o = Activator.CreateInstance<T>();
                    foreach (var h in headers)
                    {
                        var val = csvReader.GetField(h);

                        if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val) || val.ToLower() == "null")
                            continue;
                        else if (IsNumeric(val))
                        {
                            if(h.ToLower() == "price")
                                Ject.Set(o, h, decimal.Parse(val));
                            else
                                Ject.Set(o, h, int.Parse(val));
                        }
                        else
                        {
                            Ject.Set(o, h, val);
                        }
                    }
                    yield return (T)o;
                }
            }
        }

        public static bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
    }
}
