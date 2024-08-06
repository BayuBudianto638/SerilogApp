using Serilog;
using Serilog.Filters;
using SerilogApp.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog();

builder.Host.UseSerilog((context, services, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
.Enrich.FromLogContext()
.WriteTo.Console());

//Log.Logger = new LoggerConfiguration()
//.WriteTo.Console()
//.CreateLogger();

////builder.Host.UseSerilog((ctx, lc) => lc
////    .WriteTo.Console() // Write to console
////    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Write to file
////    .ReadFrom.Configuration(ctx.Configuration)
////    .Enrich.FromLogContext()
////    .WriteTo.Console()
//    .CreateLogger());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<SerilogMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
