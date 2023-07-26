using Bogus;
using XtraWork.API.Entities;
using XtraWork.API.Repositories;

namespace XtraWork.API.Extensions
{
    public static class DataGenerationExtensions
    {
        public static async Task GenerateProducts(this XtraWorkContext context, int count)
        {
            var productFaker = new Faker<Product>()
                .RuleFor(prop => prop.Name, setter => setter.Commerce.ProductName())
                .RuleFor(prop => prop.Description, setter => setter.Commerce.ProductDescription())
                .RuleFor(prop => prop.Price, setter => Convert.ToDecimal(setter.Commerce.Price()));

            var products = productFaker.Generate(count);
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}