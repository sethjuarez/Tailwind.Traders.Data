using CsvHelper;
using DataWorx.Core;
using DataWorx.Core.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tailwind.Traders.Generator.Address
{
    public class AddressEntity
    {
        //Address,City,StateProvinceCode,StateProvince,PostalCode,CountryCode,Country

        public string Address { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }

        public override string ToString()
        {
            return string.Format($"{Address} {City} {StateProvinceCode} {StateProvince} {PostalCode} {CountryCode} {Country}");
        }


        private static AddressEntity[] _addresses;
        public static AddressEntity[] Load()
        {
            if (_addresses == null)
            {
                List<AddressEntity> addresses = new List<AddressEntity>();
                var assembly = typeof(AddressEntity).Assembly;
                using (TextReader stream = new StreamReader(assembly.GetManifestResourceStream("Tailwind.Traders.Generator.Data.Address.csv")))
                using (var csvReader = new CsvReader(stream))
                {
                    csvReader.Read();
                    csvReader.ReadHeader();
                    var headers = csvReader.Context.HeaderRecord;
                    while (csvReader.Read())
                    {
                        AddressEntity address = new AddressEntity();
                        foreach (var h in headers)
                        {
                            var val = csvReader.GetField(h);
                            if (val.ToLower() != "null")
                                Ject.Set(address, h, val);
                        }
                        addresses.Add(address);
                    }
                }

                _addresses = addresses.ToArray();
            }

            return _addresses;
        }

        private readonly static List<int> _used = new List<int>();
        public static AddressEntity Random()
        {
            if (_addresses == null) Load();

            // after 80% usage reset
            if ((double)_used.Count / (double)_addresses.Length > .8) _used.Clear();

            var next = Sampling.GetUniform(_addresses.Length - 1);

            while (_used.Contains(next))
                next = Sampling.GetUniform(_addresses.Length - 1);

            _used.Add(next);

            return _addresses[next];
        }
    }
}
