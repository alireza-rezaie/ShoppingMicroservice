using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrustructure.Data
{
    public static class ProductSeedData
    {
        public static void SeedData(IMongoCollection<Product> mongoCollection)
        {
            var existCollection = mongoCollection.Find(t => true).Any();
            if (existCollection) return;
            var jsonFile = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "products.txt");
            if (!File.Exists(jsonFile))
            {
                throw new Exception($"The Seed Data Of products.txt Not Found {jsonFile}");
            }
            var DataText = File.ReadAllText(jsonFile);
            var products = JsonSerializer.Deserialize<List<Product>>(DataText);
            if (products != null) mongoCollection.InsertMany(products);
        }
    }
}
