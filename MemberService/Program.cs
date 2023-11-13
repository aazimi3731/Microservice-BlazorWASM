using MemberService.Interfaces;
using MemberService.Models;
using MemberService.Models.Interfaces;
using MemberService.Models.Repositories;
using MemberService.Services;
using MicroServices.Grpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MemberDbContextConnection") ??
  throw new InvalidOperationException("Connection string 'MemberDbContextConnection' not found.");

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); ;

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IJobCategoryService, JobCategoryService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<Members.MembersBase, MemberHelper>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<MemberServiceDbContext>(options =>
{
  options.UseSqlServer(
    builder.Configuration["ConnectionStrings:MemberDbContextConnection"]);
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
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Member GRPC API", Version = "v1" });
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
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Member API V1");
});

app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
  endpoints.MapGrpcService<MemberHelper>();
});

DbInitializer.Seed(app);

app.Run();
