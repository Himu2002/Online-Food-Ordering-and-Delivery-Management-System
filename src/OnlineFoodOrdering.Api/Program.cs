using OnlineFoodOrdering.Api.Extensions;
using OnlineFoodOrdering.Infrastructure.Persistence;
using OnlineFoodOrdering.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFoodOrderingInfrastructure(builder.Configuration);
builder.Services.AddFoodOrderingApi(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
