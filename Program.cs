using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Hosting;
using System.IO;
using JordanGardenStockWebAPI.Services;

const string OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(@"Logs/log-.log", rollingInterval: RollingInterval.Day, outputTemplate: OUTPUT_TEMPLATE)
    .CreateLogger();

try
{
    Log.Information("Starting Web API");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(Log.Logger, dispose: true);

    // Add services to the container.

    builder.Services.AddControllers();

    builder.Services.AddDbContextPool<JordanGardenStockDbContext>(
        options => options.UseNpgsql(
            builder.Configuration.GetConnectionString("JordanGardenDatabase"),
            providerOptions => { providerOptions.EnableRetryOnFailure(); }));

    builder.Services.AddTransient<TillandsiaService>();
    builder.Services.AddTransient<CompanyService>();
    builder.Services.AddTransient<ProductService>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Jordan Garden Stock API",
            Description = "An ASP.NET Core Web API for Jordan Garden's stock function"
        });

        // using System.Reflection;
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Unite/Error");
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });

    app.Run();
}
catch(Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "Stopped program because of exception");
}
finally
{
    Log.CloseAndFlush();
}