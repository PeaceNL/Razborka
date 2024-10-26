using Microsoft.EntityFrameworkCore;
using BackEndForRazborka.Models;
using DotNetEnv;
using Amazon;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BackEndForRazborka.Services;
using Prometheus;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

Env.Load();

builder.Services.AddSingleton<S3Service>(provider => new S3Service(
    accessKey: Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") 
        ?? throw new ArgumentException("no access key"),
    secretKey: Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") 
        ?? throw new ArgumentException("no secret key"),
    bucketName: Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME")
        ?? throw new ArgumentException("no bucket name"),
    region: Environment.GetEnvironmentVariable("AWS_REGION")
        ?? throw new ArgumentException("no region")
));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,        
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
            ?? throw new InvalidOperationException("JWT_SECRET_KEY must be set.")))
    };
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var httpReuestCounter = Metrics.CreateCounter("htttp_requet_total", "total number of http request");

app.Use(async (context, next) =>
{
    httpReuestCounter.Inc();
    await next();
});

app.UseMetricServer();

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
