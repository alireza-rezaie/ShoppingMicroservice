using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Infrustructure.Services
{
    public class DiscountRepository(IConfiguration configuration) : IDiscountRepository
    {
        private readonly string? connectionString = configuration.GetConnectionString("DiscountDb");

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = "Insert Into Coupon (ProductName,ProductId,Description,Amount) values (@ProductName,@ProductId,@Description,@Amount)";
            var affected = await connection.ExecuteAsync(sql, new { coupon.ProductName,coupon.ProductId,coupon.Description,coupon.Amount });
            return (affected > 0);
        }
        public async Task<Coupon> GetDiscountByName(string productName)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var sql = "Select * From Coupon Where ProductName = @productName";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, new { productName = productName });
            return coupon ?? new Coupon();
        }
        public async Task<Coupon> GetDiscountById(string productId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            var sql = "Select * From Coupon Where Id = @productId";
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, new { productId = productId });
            return coupon ?? new Coupon();
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = "Update Coupon Set ProductName = @ProductName,ProductId = @ProductId,Description = @Description,Amount = @Amount Where Id = @Id";
            var affected = await connection.ExecuteAsync(sql, new { coupon.ProductName, coupon.ProductId, coupon.Description, coupon.Amount, coupon.Id });
            return (affected > 0);
        }
        public async Task<bool> DeleteDiscount(string productId)
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = "Delete From Coupon Where ProductId = @productName";
            var affected = await connection.ExecuteAsync(sql, new { ProductId = productId });
            return (affected > 0);
        }
        public async Task<bool> DeleteDiscountByProductName(string productName)
        {
            using var connection = new NpgsqlConnection(connectionString);
            const string sql = "Delete From Coupon Where ProductName = @productName";
            var affected = await connection.ExecuteAsync(sql, new { ProductName = productName });
            return (affected > 0);
        }
    }
}
