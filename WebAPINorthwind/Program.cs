using Microsoft.EntityFrameworkCore;
using WebAPINorthwind.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NorthwindContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("BDNwind"),
        ServerVersion.Parse("8.0.17-mysql"));
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:8080");
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
