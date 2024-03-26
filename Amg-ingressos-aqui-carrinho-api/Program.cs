using System.Globalization;
using System.Text.Json.Serialization;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Repository;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
// Add services to the container.
builder.Services.Configure<TransactionDatabaseSettings>(
    builder.Configuration.GetSection("CarrinhoDatabase"));
builder.Services.Configure<PaymentSettings>(
    builder.Configuration.GetSection("PaymentSettings"));


// injecao de dependencia
//services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ITransactionItenService, TransactionItenService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();


//repository
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionItenRepository, TransactionItenRepository>();

//infra
builder.Services.AddScoped<IDbConnection, DbConnection>();
builder.Services.AddScoped<ITransactionGatewayClient, CieloClient>();
builder.Services.AddScoped<ITransactionGatewayClient, PagBankClient>();
builder.Services.AddScoped<IPagbankService, PagbankService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var defaultDateCulture = "pt-BR";
var ci = new CultureInfo(defaultDateCulture);
ci.NumberFormat.NumberDecimalSeparator = ",";
ci.NumberFormat.CurrencyDecimalSeparator = ",";



var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the Localization middleware
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(ci),
    SupportedCultures = new List<CultureInfo>
    {
        ci,
    },
    SupportedUICultures = new List<CultureInfo>
    {
        ci,
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Certifique-se de ter essa linha
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<TransactionExceptionHandlerMiddleaware>();
app.Run();
