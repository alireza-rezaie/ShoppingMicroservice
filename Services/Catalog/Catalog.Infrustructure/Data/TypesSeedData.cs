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
    public static class TypesSeedData
    {
        public static void SeedData(IMongoCollection<ProductType> mongoCollection)
        {
            var existCollection = mongoCollection.Find(t => true).Any();
            if (existCollection) return;
            var jsonFile = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "types.txt");
            if (!File.Exists(jsonFile))
            {
                throw new Exception($"The Seed Data Of types.txt Not Found {jsonFile}");
            }
            var DataText = File.ReadAllText(jsonFile);
            var types = JsonSerializer.Deserialize<List<ProductType>>(DataText);
            if (types != null) mongoCollection.InsertMany(types);
        }
    }
}
