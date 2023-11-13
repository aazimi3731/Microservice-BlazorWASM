using MicroServices.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderShoppingService.Models;
using OrderShoppingService.Models.Interfaces;
using OrderShoppingService.Models.Repositories;
using OrderShoppingService.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrderShoppingDbContextConnection") ??
  throw new InvalidOperationException("Connection string 'OrderShoppingDbContextConnection' not found.");

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ShoppingCartItems.ShoppingCartItemsBase, ShoppingCartService>();
builder.Services.AddScoped<Orders.OrdersBase, OrderService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<OrderShoppingServiceDbContext>(options =>
{
  options.UseSqlServer(
    builder.Configuration["ConnectionStrings:OrderShoppingDbContextConnection"]);
});

builder.Services.AddControllers();

builder.Services.AddGrpc(options => {
  options.EnableDetailedErrors = true;
  options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
  options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
}).AddJsonTranscoding();

//builder.Services.AddCors(setupAction => {
//  setupAction.AddDefaultPolicy(policy => {
//    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
//  });
//});

builder.Services.AddCors(policy => {
  policy.AddPolicy("Policy_Name", builder =>
    builder.WithOrigins("https://localhost:7265/")
      .SetIsOriginAllowedToAllowWildcardSubdomains()
      .AllowAnyOrigin()
      .AllowAnyHeader()
      .AllowAnyMethod()
    );
});

builder.Services.AddGrpcSwagger().AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Category GRPC API", Version = "v1" });
});

var app = builder.Build();

//app.UseCors();
// Configure the HTTP request pipeline.
app.UseCors("Policy_Name");

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

app.UseHttpsRedirection();

app.UseRouting();

app.UseGrpcWeb(new GrpcWebOptions
{
  DefaultEnabled = true
});

app.UseSwagger().UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order ShoppingCart API V1");
});

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
  endpoints.MapGrpcService<OrderService>();
  endpoints.MapGrpcService<ShoppingCartService>();
});

app.Run();
