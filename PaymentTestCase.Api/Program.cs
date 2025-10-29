using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentIntegration.Infrastructure.Persistence;
using PaymentTestCase.Application.Commands.Transaction;
using PaymentTestCase.Application.Interfaces;
using PaymentTestCase.Application.Mapping;
using PaymentTestCase.Application.Services;
using PaymentTestCase.Domain.Abstract;
using PaymentTestCase.Domain.Abstract.Repositories;
using PaymentTestCase.Domain.Rules;
using PaymentTestCase.Infrastructure.Banks;
using PaymentTestCase.Infrastructure.Persistence;
using PaymentTestCase.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddScoped<IPaymentDbContext>(sp => sp.GetRequiredService<PaymentDbContext>());

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<AkbankGateway>();
builder.Services.AddScoped<GarantiGateway>();
builder.Services.AddScoped<YapiKrediGateway>();

builder.Services.AddScoped<IBankGatewayFactory, BankGatewayFactory>();
builder.Services.AddScoped<IBankRulesFactory, BankRulesFactory>();

builder.Services.AddScoped<AkbankRules>();
builder.Services.AddScoped<GarantiRules>();
builder.Services.AddScoped<YapiKrediRules>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(PayCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
