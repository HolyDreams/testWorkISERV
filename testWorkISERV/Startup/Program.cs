using Microsoft.Extensions.DependencyInjection;
using testWorkISERV.Services.Country;
using testWorkISERV.Services.Data;
using testWorkISERV.Services.ETL;
using testWorkISERV.Services.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEditCountryService, EditCountryService>();
builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddSingleton<IEtlService, EtlService>();

builder.Services.AddHostedService<EtlAutoUpdateService>();

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