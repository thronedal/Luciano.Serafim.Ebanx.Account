using Luciano.Serafim.Ebanx.Account.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEbanxAll(builder.Configuration);

var app = builder.Build();

app.UseEbanxApi();

app.Run();