using Amg_ingressos_aqui_carrinho_api.Infra;
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

// injecao de dependencia
//services
builder.Services.AddScoped<ITransactionService, TransactionService>();
//repository
builder.Services.AddScoped<ITransactionRepository, TransactionRepository<object>>();
//infra
builder.Services.AddScoped<IDbConnection, DbConnection>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
