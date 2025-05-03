var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 添加 CORS 政策
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 使用 CORS 政策
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// 添加根路徑回應
app.MapGet("/", () => Results.Json(new { message = "Miraishop API Server", status = "running", timestamp = DateTime.Now }));

app.MapFallbackToFile("/index.html");

app.Run();
