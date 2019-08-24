using AutoMapper;
using Tailwind.Traders.Data;
using DataWorx.Core;
using DataWorx.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using DataWorx.Core.Graph;
using System.Linq;
using DataWorx.Core.Math;
using DataWorx.Core.Distribution;

namespace Tailwind.Traders.Generator
{
    public class Generate
    {
        private readonly IMapper _mapper;
        public Generate()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonGen, Person>();
                cfg.CreateMap<CompanyGen, Company>();
            });
            _mapper = configuration.CreateMapper();
        }

        private ObjectGraph _graph;
        public void Create()
        {
            // load products
            var products = Loader.LoadProductGraph();

            // types to build
            var types = new[] { typeof(CompanyGen), typeof(PersonGen) };
            var graph = new ObjectGraph(types);

            var people = GetPeople(graph);
            
        }

        public List<Person> GetPeople(ObjectGraph graph)
        {
            // get companies
            var companies = graph.Find<CompanyGen>()
                                 .GenerateSet<CompanyGen>()
                                 .Select(c => _mapper.Map<Company>(c))
                                 .ToList();

            // generate people
            var people = graph.Find<PersonGen>()
                                 .GenerateSet<PersonGen>()
                                 .Select(p => _mapper.Map<Person>(p))
                                 .ToList();

            // draw from companies Normally at random
            var normalDraw = new NormalDraw();
            foreach (var p in people)
            {
                p.Company = normalDraw.Draw<Company>(companies, companies.Count);
                p.Email = $"{p.FirstName.ToLower()}.{p.LastName.ToLower()}@{p.Company.Name.ToLower()}.com";
            }

            return people;
        }
    }
}
