using ecommerce.Helpers;
using Microsoft.EntityFrameworkCore;
using ecommerce.Models;
using ecommerce.Data;
using ecommerce.Cache;
using ecommerce.Services;

var builder = WebApplication.CreateBuilder(args);

//ConnectionString for connecting database
var connectionString = builder.Configuration.GetConnectionString("eCommerceDB");
builder.Services.AddDbContextPool<eCommerceDbContext>(option =>
option.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString))
);
// Add services to the container.
builder.Services.AddMemoryCache(); 
builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(options => options.RootPath = "frontapp/dist");
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<eCommerceDbContext>();
    dataContext.Database.Migrate();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = "frontapp";
    if (app.Environment.IsDevelopment())
    {
        // Launch development server for Nuxt
        spa.UseNuxtDevelopmentServer();
    }
});

app.Run();
