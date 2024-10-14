using System.Text;
using DuitKu.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

// Come from user-secrets
builder.Configuration["ConnectionStrings:DefaultConnection"] = builder.Configuration["DB"]; 
builder.Configuration["Jwt:Key"] = builder.Configuration["JWT_KEY"]; 

var SecretKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

builder.Services
    .Boot()
    .AddJWTAuthentication(SecretKey)
    // .AddCorsConfig()
    .AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Boot().Middleware().Run();