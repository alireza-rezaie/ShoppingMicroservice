using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrustructure.Data
{
    public static class BrandSeedData
    {
        public static void SeedData(IMongoCollection<ProductBrand> mongoCollection)
        {
            var existCollection = mongoCollection.Find(t => true).Any();
            if (existCollection) return;
            var jsonFile = Path.Combine(AppContext.BaseDirectory, "Data","SeedData","brands.txt");
            if (!File.Exists(jsonFile))
                throw new Exception($"The Seed Data Of Brands.txt Not Found {jsonFile}");

            var DataText = File.ReadAllText(jsonFile);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(DataText);
            if (brands != null) mongoCollection.InsertMany(brands);
        }
    }
}
