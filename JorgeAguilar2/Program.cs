using JorgeAguilar2;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ProductDB>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", async (ProductDB productDB) => await productDB.Products.ToListAsync());

app.MapPost("/products", async (ProductDB productDB, Product product) =>
{
    await productDB.Products.AddAsync(product);
    await productDB.SaveChangesAsync();
    return Results.Created($"/products/{product.Id}", product);
});

app.MapPut("/product/{id}", async (ProductDB productDB, Product product, int id) =>
{
    var productItem = await productDB.Products.FindAsync(id);
    if (productItem is null) return Results.NotFound();
    productItem.Name = product.Name;
    productItem.Description = product.Description;
    await productDB.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/products/{id}", async (ProductDB productDB, int id) =>
{
    var productItem = await productDB.Products.FindAsync(id);
    if (productItem is null) return Results.NotFound();
    productDB.Products.Remove(productItem);
    await productDB.SaveChangesAsync();
    return Results.NoContent();
});


app.Run();
