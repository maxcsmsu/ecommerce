using AutoMapper;
using ecommerce.Models;

namespace ecommerce.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateUpdateProductRequest -> Product
            CreateMap<CreateUpdateProductRequest, Product>();
        }
    }
}
