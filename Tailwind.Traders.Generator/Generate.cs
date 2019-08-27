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
        private readonly List<Products> _products;
        private readonly List<Person> _people;
        private readonly DateDraw _dateDistribution;
        public Generate()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonGen, Person>();
                cfg.CreateMap<CompanyGen, Company>();
            });
            _mapper = configuration.CreateMapper();
            // load products

            // types to build
            var types = new[] { typeof(CompanyGen), typeof(PersonGen) };
            var graph = new ObjectGraph(types);

            _people = People(graph);

            _products = Loader.LoadProductGraph();
            _dateDistribution = new DateDraw();
            _dateDistribution.SetDayOfWeek(1, 8, 7, 5, 3, 2, 1);
            _dateDistribution.SetMonth(5, 4, 3, 4, 5, 6, 1, 2, 6, 7, 3, 1);
            _dateDistribution.SetHour(1, 1, 1, 1, 1, 1, 2, 3, 5, 8, 10, 15, 17, 3, 10, 11, 12, 20, 5, 4, 3, 2, 1, 1);
        }

        

        public List<Person> People(ObjectGraph graph)
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

        public IEnumerable<Invoice> Invoices(int year, int min, int max, int start)
        {
            return Invoices(year, Sampling.GetUniform(min, max), start);
        }
        
        public IEnumerable<Invoice> Invoices(int year, int count, int start)
        {
            // create sampling distribution (normal)
            var normalDraw = new NormalDraw();

            // create discount distribution
            var tax = .07m;
            var discreteDraw = new DiscreteDraw(new[] { 7d, 2, 1 });

            for (int i = 0; i < count; i++)
            {
                Invoice invoice = new Invoice
                {
                    InvoiceNumber = ++start,
                    Customer = normalDraw.Draw<Person>(_people, _people.Count),
                    OrderDate = _dateDistribution.Draw(year),
                    LineItems = new List<LineItem>()
                };

                for (int j = 0; j < Sampling.GetUniform(1, 5); j++)
                {
                    LineItem line = new LineItem()
                    {
                        Product = normalDraw.Draw<Products>(_products, _products.Count),
                        Quantity = Sampling.GetUniform(1, 3),
                        Discount = discreteDraw.Draw(3) * .1m,
                    };
                    
                    line.DiscountTotal = line.Quantity * line.Product.Price * line.Discount;
                    line.SubTotal = (line.Quantity * line.Product.Price) - line.DiscountTotal;
                    line.Tax = line.SubTotal * tax;
                    line.LineTotal = line.SubTotal + line.Tax;
                    
                    invoice.LineItems.Add(line);
                }

                invoice.SubTotal = invoice.LineItems.Sum(l => l.SubTotal);
                invoice.DiscountTotal = invoice.LineItems.Sum(l => l.DiscountTotal);
                invoice.TaxTotal = invoice.LineItems.Sum(l => l.Tax);
                invoice.Total = invoice.LineItems.Sum(l => l.LineTotal);

                yield return invoice;
            }
        }
    }
}
