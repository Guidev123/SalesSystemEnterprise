using SalesSystem.API.Configuration;
using SalesSystem.Catalog.Infrastructure;
using SalesSystem.Payments.Infrastructure;
using SalesSystem.Register.Infrastructure;
using SalesSystem.Sales.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurations();
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddSalesModule(builder.Configuration);
builder.Services.AddPaymentsModule(builder.Configuration);
builder.Services.AddRegisterModule(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.AddApiUsing();

app.Run();