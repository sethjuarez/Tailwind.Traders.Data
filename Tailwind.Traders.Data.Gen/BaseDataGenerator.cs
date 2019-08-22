using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Tailwind.Traders.Data.Gen
{
    public class BaseDataGenerator
    {
        private readonly Dictionary<string, string> _tables;
        private readonly Dictionary<string, Func<string>> _createTables;
        public BaseDataGenerator()
        {
            _tables = new Dictionary<string, string>
            {
                { "Brands", "https://raw.githubusercontent.com/microsoft/TailwindTraders-Backend/master/Source/Services/Tailwind.Traders.Product.Api/Setup/ProductBrands.csv" },
                { "Types", "https://raw.githubusercontent.com/microsoft/TailwindTraders-Backend/master/Source/Services/Tailwind.Traders.Product.Api/Setup/ProductTypes.csv" },
                { "Tags", "https://raw.githubusercontent.com/microsoft/TailwindTraders-Backend/master/Source/Services/Tailwind.Traders.Product.Api/Setup/ProductTags.csv" },
                { "Features", "https://raw.githubusercontent.com/microsoft/TailwindTraders-Backend/master/Source/Services/Tailwind.Traders.Product.Api/Setup/ProductFeatures.csv" },
                { "Products", "https://raw.githubusercontent.com/microsoft/TailwindTraders-Backend/master/Source/Services/Tailwind.Traders.Product.Api/Setup/ProductItems.csv" }
            };

            _createTables = new Dictionary<string, Func<string>>
            {
                { "Brands", CreateBrandsTable },
                { "Types", CreateTypesTable },
                { "Tags", CreateTagsTable },
                { "Features", CreateFeaturesTable },
                { "Products", CreateProductsTable }
            };

        }

        public void Seed()
        {
            foreach (var key in _createTables.Keys)
            {
                Console.WriteLine($"-- Creating {key} table...");
                Console.WriteLine($"{_createTables[key].Invoke()};\nGO;");
                Console.WriteLine($"\n-- {key} Data...");
                foreach (var item in SeedTable(key, _tables[key]))
                    Console.WriteLine($"{item};");

                Console.WriteLine("GO;");
            }
        }

        private string CreateBrandsTable()
        {
            var command = @"
                CREATE TABLE Brands (
                    Id INT NOT NULL PRIMARY KEY,
                    Name NVARCHAR(255)
                )";
            return command;
        }

        private string CreateTypesTable()
        {
            var command = @"
                CREATE TABLE Types (
                    Id INT NOT NULL PRIMARY KEY,
                    Code NVARCHAR(255),
                    Name NVARCHAR(255)
                )";
            return command;
        }

        private string CreateTagsTable()
        {
            var command = @"
                CREATE TABLE Tags (
                    Id INT NOT NULL PRIMARY KEY,
                    Value NVARCHAR(255)
                )";
            return command;
        }

        private string CreateFeaturesTable()
        {
            var command = @"
                CREATE TABLE Features (
                    Id INT NOT NULL PRIMARY KEY,
                    Title NVARCHAR(255),
                    Description NVARCHAR(MAX),
                    ProductItemId INT
                )";
            return command;
        }

        private string CreateProductsTable()
        {
            var command = @"
                CREATE TABLE Products (
                    Id INT NOT NULL PRIMARY KEY,
                    Name NVARCHAR(255),
                    Price DECIMAL(9, 2),
                    ImageName NVARCHAR(255),
                    BrandId INT,
                    TypeId INT,
                    TagId INT
                )";
            return command;
        }

        private IEnumerable<string> SeedTable(string tableName, string csvUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var task = httpClient.GetStringAsync(csvUrl);
                var csv = task.Result;

                using (var stringReader = new StringReader(csv))
                using (var csvReader = new CsvReader(stringReader))
                {
                    csvReader.Read();
                    csvReader.ReadHeader();
                    var headers = csvReader.Context.HeaderRecord;

                    while (csvReader.Read())
                    {
                        var sql = new StringBuilder();
                        sql.Append($"INSERT INTO {tableName} (");
                        sql.Append(string.Join(", ", headers));
                        sql.Append(") VALUES (");
                        sql.Append(string.Join(", ", headers.Select(h => {
                            var item = csvReader.GetField(h);
                            if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                                return "null";
                            else if (IsNumeric(item)) return item;
                            else return $"\"{item}\"";
                        })));
                        sql.Append(")");
                        yield return sql.ToString();
                    }
                }
            }
        }

        public bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }
    }
}
