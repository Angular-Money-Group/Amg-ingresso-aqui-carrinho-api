using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
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
builder.Services.AddScoped<IEmailService, EmailService>();

//repository
builder.Services.AddScoped<ITransactionRepository, TransactionRepository<object>>();
builder.Services.AddScoped<ITransactionItenRepository, TransactionItenRepository<object>>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();

//infra
builder.Services.AddScoped<IDbConnection<Transaction>, DbConnection<Transaction>>();
builder.Services.AddScoped<IDbConnection<TransactionIten>, DbConnection<TransactionIten>>();
builder.Services.AddScoped<IDbConnection<Email>, DbConnection<Email>>();
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

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

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

app.Run();
