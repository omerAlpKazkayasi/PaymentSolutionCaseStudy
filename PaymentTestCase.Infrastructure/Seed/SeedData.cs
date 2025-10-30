using Microsoft.EntityFrameworkCore;
using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Domain.Constants;
using PaymentTestCase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTestCase.Infrastructure.Seed;

public static class SeedData
{
    public static async Task SeedAsync(PaymentDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await context.Products.AnyAsync())
        {
            var products = new Product(name: "Product1", 100);
            var product2 = new Product(name: "Product2", 200);

            await context.Products.AddAsync(products);
            await context.Products.AddAsync(product2);
            await context.SaveChangesAsync();
        }
        if (!await context.Stocks.AnyAsync())
        {
            var stocks = await context.Products
                .Select(p => new Stock
                (p.Id, 100)).ToListAsync();

            await context.Stocks.AddRangeAsync(stocks);
            await context.SaveChangesAsync();
        }

        if (!await context.Orders.AnyAsync())
        {
            var firstProduct = await context.Products.FirstAsync();

            var order = new Order(orderNumber: 1, status :OrderStatuses.Waiting);
            order.AddItem(firstProduct, 10);

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
    }
}