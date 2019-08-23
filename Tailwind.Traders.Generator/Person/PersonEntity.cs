using CsvHelper;
using DataWorx.Core;
using DataWorx.Core.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tailwind.Traders.Generator.Person
{
    public class PersonEntity
    {
        //Title,FirstName,MiddleName,LastName,Suffix

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }


        public override string ToString()
        {
            return string.Format($"{Title} {FirstName} {MiddleName} {LastName} {Suffix}");
        }


        private static PersonEntity[] _people;
        public static PersonEntity[] Load()
        {
            if (_people == null)
            {
                List<PersonEntity> people = new List<PersonEntity>();
                var assembly = typeof(PersonEntity).Assembly;
                using (TextReader stream = new StreamReader(assembly.GetManifestResourceStream("Tailwind.Traders.Generator.Data.Person.csv")))
                using (var csvReader = new CsvReader(stream))
                {
                    csvReader.Read();
                    csvReader.ReadHeader();
                    var headers = csvReader.Context.HeaderRecord;
                    while (csvReader.Read())
                    {
                        PersonEntity person = new PersonEntity();
                        foreach (var h in headers)
                        {
                            var val = csvReader.GetField(h);
                            if(val.ToLower() != "null")
                                Ject.Set(person, h, val);
                        }
                        people.Add(person);
                    }
                }

                _people = people.ToArray();
            }

            return _people;
        }

        private readonly static List<int> _used = new List<int>();
        public static PersonEntity Random()
        {
            if (_people == null) Load();

            // after 80% usage reset
            if ((double)_used.Count / (double)_people.Length > .8) _used.Clear();

            var next = Sampling.GetUniform(_people.Length - 1);

            while (_used.Contains(next))
                next = Sampling.GetUniform(_people.Length - 1);

            _used.Add(next);

            return _people[next];
        }
    }
}
