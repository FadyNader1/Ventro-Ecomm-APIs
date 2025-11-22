using Ecomm.Errors;
using Ecomm.Extensions;
using Ecomm.Middleware;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppService(builder.Configuration);
builder.Services.AddSwaggerService();


var app = builder.Build();

await app.UseAppService();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerService();
}

app.UseMiddleware<SecurityMiddleware>();
app.UseRateLimiter();
app.UseMiddleware<MiddlewareHandleError>();
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers().RequireRateLimiting("fixed");

app.Run();
