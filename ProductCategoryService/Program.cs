using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductCategoryService;
using ProductCategoryService.Models;
using ProductCategoryService.Models.Interfaces;
using ProductCategoryService.Models.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PieCategoryDbContextConnection") ??
  throw new InvalidOperationException("Connection string 'PieCategoryDbContextConnection' not found.");

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPieService, PieService>();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ProductCategoryServiceDbContext>(options =>
{
  options.UseSqlServer(
    builder.Configuration["ConnectionStrings:PieCategoryDbContextConnection"]);
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
  c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Category API", Version = "v1" });
});

builder.Services.AddCors(policy => {
  policy.AddPolicy("Policy_Name", builder =>
    builder.WithOrigins("https://localhost:7265/")
      .SetIsOriginAllowedToAllowWildcardSubdomains()
      .AllowAnyOrigin()
    );
});

var app = builder.Build();

//
// Middelwares
//
app.UseStaticFiles();

app.UseSession();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
else
{
  app.UseHsts();
}

// Configure the HTTP request pipeline.
app.UseCors("Policy_Name");

app.UseHttpsRedirection();

app.UseRouting();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Category API V1");
});

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});

DbInitializer.Seed(app);

app.Run();
