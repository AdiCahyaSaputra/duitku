using System.Text;
using DuitKu.Bootstrap;

var builder = WebApplication.CreateBuilder(args);
var SecretKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

builder.Services
    .Boot()
    .AddJWTAuthentication(SecretKey)
    .AddCorsConfig()
    .AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Boot().Middleware().Run();