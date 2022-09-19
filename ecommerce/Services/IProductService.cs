using ecommerce.Models;

namespace ecommerce.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(long id);
        Task Create(CreateUpdateProductRequest model);
        Task Update(long id, CreateUpdateProductRequest model);
        Task Delete(long id);
    }
}
