using Microsoft.EntityFrameworkCore;
using OrderApplication;
using OrderApplication.Helpers;
using OrderApplication.Services.HttpService;
using OrderApplication.Services.OrderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


var microserviceConfiguration = builder.Configuration.GetSection("MicroserviceConfiguration").Get<MicroserviceConfiguration>();

builder.Services.AddSingleton(microserviceConfiguration);
//builder.Services.AddHttpClient();

builder.Services.AddHttpClient<IHttpService, HttpService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// add cors to make sure it works with all url (FE)
builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyOrigin();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
