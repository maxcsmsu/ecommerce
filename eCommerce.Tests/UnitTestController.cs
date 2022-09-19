using ecommerce.Cache;
using ecommerce.Controllers;
using ecommerce.Models;
using ecommerce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace eCommerce.Tests
{
    public class UnitTestController
    {
        private readonly Mock<IProductService> productService;
        private readonly Mock<IConfiguration> mockConfig;
        private readonly Mock<ILogger<ProductController>> mocklogger;
        private readonly Mock<ICacheService> mockCacheService;
        public UnitTestController()
        {
            productService = new Mock<IProductService>();
            mockConfig = new Mock<IConfiguration>();
            mocklogger = new Mock<ILogger<ProductController>>();
            mockCacheService = new Mock<ICacheService>();
        }

        [SetUp]
        public void Setup()
        {
            mockConfig.SetupGet(x => x[It.Is<string>(s => s == "Configuration:ExpirationTime")]).Returns("5");
            
        }
        [Test]
        public async Task GetProductListAsync()
        {
            //arrange
            var productList = GetProductsData();
            productService.Setup(x => x.GetAll())
                .Returns(productList);
            var productController = new ProductController((IConfiguration)mockConfig, (ILogger<ProductController>)mocklogger, (ICacheService)mockCacheService, productService.Object);

            var getname = new ProductRequestDto()
            {
                Name=""
            };
            //act
            var productResult =await productController.GetAll(getname);
            var okResult = productResult as OkObjectResult;

            //assert
            Assert.NotNull(productResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.True(productList.Equals(productResult));
        }

        private async Task<List<Product>> GetProductsData()
        {

            List<Product> productsData = new List<Product>
        {       new Product { Id = 1, Name = "�����Ѻ��ԧ", Description = ""},
                new Product { Id = 2, Name ="������µ��", Description = ""},
                new Product { Id = 3, Name = "���ö�����", Description = ""},
                new Product { Id = 4, Name = "��ǹ�ѹ������͹", Description = ""},
                new Product { Id = 5, Name = "෻�ѹ���", Description = ""},
                new Product { Id = 6, Name = "����⤹", Description = ""},
                new Product { Id = 7, Name = "����ҧ", Description = ""},
        };
            return await Task.Run(()=> productsData);
        }
    }
}