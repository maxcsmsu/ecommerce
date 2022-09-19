using AutoMapper;
using ecommerce.Data;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ecommerce.Services
{
    public class ProductService : IProductService
    {
        private eCommerceDbContext _context;
        private readonly IMapper _mapper;
        private static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;
        public ProductService(
            eCommerceDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Product>> GetAll()
        {
                var productlist = await _context.Products.ToListAsync();
                return productlist;
        }

        public async Task<Product> GetById(long id)
        {
            return await getProduct(id);
        }

        public async Task Create(CreateUpdateProductRequest model)
        {
            // validate
            if (_context.Products.Any(x => x.Name == model.Name))
                throw new Exception("Product with the name '" + model.Name + "' already exists");

            // map model to new Product object
            var Product = _mapper.Map<Product>(model);

            // save Product
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
        }

        public async Task Update(long id, CreateUpdateProductRequest model)
        {
            var Product = await getProduct(id);
            if (Product != null)
            {
                // validate
                if (model.Name != Product.Name && _context.Products.Any(x => x.Name == model.Name  && x.Id != id))
                    throw new Exception("Product with the name '" + model.Name + "' already exists");

                // copy model to Product and save
                _mapper.Map(model, Product);
                _context.Products.Update(Product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(long id)
        {
            var Product = await getProduct(id);
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
        }

        // helper methods

        private async Task<Product> getProduct(long id)
        {
            var Product = await _context.Products.FindAsync(id);
            if (Product == null) throw new KeyNotFoundException("Product not found");
            return Product;
        }
    }
}