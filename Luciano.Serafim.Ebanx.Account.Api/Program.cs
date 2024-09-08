using Luciano.Serafim.Ebanx.Account.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEbanxAll();

var app = builder.Build();

app.UseEbanxApi();

app.Run();