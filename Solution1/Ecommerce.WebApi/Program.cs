using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.IRepositories;
using Ecommerce.Infrastructure.Repositories;
using JwtAuth;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var _policyName = "CorsPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: _policyName, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
               .AddEntityFrameworkStores<EcommerceDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(_policyName);
app.UseAuthorization();

app.MapControllers();

app.Run();
