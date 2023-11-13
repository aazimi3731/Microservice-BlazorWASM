using AutoMapper;
using Blazored.SessionStorage;
using ClientApp;
using ClientApp.Components;
using ClientApp.Helpers;
using ClientApp.Interfaces;
using ClientApp.Services;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using MicroServices.Grpc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Web.WebPages.Html;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var azureConnectionString = builder.Configuration["AzureQueues:AzureConnectionString"] ?? 
  throw new InvalidOperationException("Connection string 'AzureConnectionString' not found.");

var azureQueueName = builder.Configuration["AzureQueues:AzureQueueName"] ??
  throw new InvalidOperationException("Connection string 'AzureQueueName' not found.");

PaymentServiceImpl.SetStringParameters(azureConnectionString, azureQueueName);

builder.Services.AddTelerikBlazor();

builder.Services.AddHttpClient<IMicroserviceReqResp, MicroserviceReqResp>(client => client.BaseAddress = new Uri(builder.Configuration["ApiConfigs:ProductCategory:Uri"]));

builder.Services.AddSingleton(services =>
{
var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
var baseUri = new Uri(builder.Configuration["ApiConfigs:OrderShoppingCart:Uri"] ?? string.Empty);
var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
return new Orders.OrdersClient(channel);
});

builder.Services.AddSingleton(services =>
{
var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
var baseUri = new Uri(builder.Configuration["ApiConfigs:OrderShoppingCart:Uri"] ?? string.Empty);
var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
return new ShoppingCartItems.ShoppingCartItemsClient(channel);
});

builder.Services.AddSingleton(services =>
{
var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
var baseUri = new Uri(builder.Configuration["ApiConfigs:Member:Uri"] ?? string.Empty);
var channel = GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions { HttpClient = httpClient });
return new Members.MembersClient(channel);
});

builder.Services.AddScoped<GlobalItems>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IPaymentService, PaymentServiceImpl>();
builder.Services.AddScoped<IShoppingCartItemService, ShoppingCartItemService>(sp => ShoppingCartItemService.SetServices(sp,
  sp.GetService<UserManager<IdentityUser>>() ?? null, sp.GetService<ShoppingCartItems.ShoppingCartItemsClient>() ?? null,
  sp.GetService<IProductService>() ?? null, sp.GetService<IMapper>() ?? null));
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IJobCategoryService, JobCategoryService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddScoped<CategoryMenu>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//For simplification
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//builder.Services.AddDefaultIdentity<IdentityUser>()
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<ProjectDbContext>();


//builder.Services.AddGrpcClient<Orders.OrdersClient>(
//    o => o.Address = new Uri(builder.Configuration["ApiConfigs:OrderShoppingCart:Uri"] ?? string.Empty));

//builder.Services.AddGrpcClient<ShoppingCartItems.ShoppingCartItemsClient>(
//    o => o.Address = new Uri(builder.Configuration["ApiConfigs:OrderShoppingCart:Uri"] ?? string.Empty));

//builder.Services.AddGrpcClient<Members.MembersClient>(
//    o => o.Address = new Uri(builder.Configuration["ApiConfigs:Member:Uri"] ?? string.Empty));

builder.Services.AddBlazoredSessionStorage();

var app = builder.Build();

await builder.Build().RunAsync();
