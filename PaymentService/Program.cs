using Azure.Messaging.ServiceBus;
using Microsoft.OpenApi.Models;
using PaymentService;
using PaymentService.Services;
using System.Text.Json.Serialization;

Tuple<ServiceBusClient, ServiceBusProcessor>? serviceBus = null;

try
{
  var builder = WebApplication.CreateBuilder(args);

  //var connectionString = builder.Configuration.GetConnectionString("PaymentDbContextConnection") ??
  //  throw new InvalidOperationException("Connection string 'PaymentDbContextConnection' not found.");

  builder.Services.AddControllersWithViews()
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      }); ;

  builder.Services.AddSingleton<BusReceiverService>();

  builder.Services.AddSession();

  builder.Services.AddHttpContextAccessor();

  //builder.Services.AddDbContext<PaymentServiceDbContext>(options =>
  //{
  //  options.UseSqlServer(
  //    builder.Configuration["ConnectionStrings:PaymentDbContextConnection"]);
  //});

  builder.Services.AddControllers();

  builder.Services.AddSwaggerGen(c =>
  {
    c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
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

  app.UseHttpsRedirection();

  app.UseRouting();

  app.UseSwagger();

  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API V1");
  });

  app.UseEndpoints(endpoints =>
  {
    endpoints.MapControllers();
  });

  serviceBus = await BusReceiverService.Initialize();

  app.Run();
}
catch(Exception ex)
{
  Console.WriteLine($"There was an error in the receiver, {ex.Message}");
}
finally
{
  // stop processing 
  if(serviceBus != null && serviceBus.Item2 != null)
  {
    Console.WriteLine("\nStopping the receiver...");
    await serviceBus.Item2.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
  }

  // Calling DisposeAsync on client types is required to ensure that network
  // resources and other unmanaged objects are properly cleaned up.
  if (serviceBus != null)
  {
    if (serviceBus.Item2 != null)
    {
      await serviceBus.Item2.DisposeAsync();
    }

    if (serviceBus.Item1 != null)
    {
      await serviceBus.Item1.DisposeAsync();
    }
  }
}
