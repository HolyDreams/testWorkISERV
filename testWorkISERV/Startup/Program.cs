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

builder.Services.AddSingleton<EtlService, EtlService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

var etl = new EtlService();
var etlTask = new Thread(() => etl.StartAsync());
etlTask.Start();

app.Run();