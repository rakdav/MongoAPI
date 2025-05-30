using Lab3.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration[
        "ConnectionStrings:ProductConnection"]);
    opts.EnableSensitiveDataLogging(true);
});
builder.Services.AddControllers();
builder.Services.AddRateLimiter(opts => {
    opts.AddFixedWindowLimiter("fixedWindow", fixOpts => {
        fixOpts.PermitLimit = 1;
        fixOpts.QueueLimit = 0;
        fixOpts.Window = TimeSpan.FromSeconds(15);
    });
});
builder.Services.Configure<MvcNewtonsoftJsonOptions>(opts => {
    opts.SerializerSettings.NullValueHandling
    = Newtonsoft.Json.NullValueHandling.Ignore;
});
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidIssuer=AuthOptions.ISSUER,
        ValidateAudience=true,
        ValidAudience=AuthOptions.AUDIENCE,
        ValidateLifetime=true,
        IssuerSigningKey=AuthOptions.GetSimmetricSecurutyKey(),
        ValidateIssuerSigningKey=true
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<Lab3.TestMiddleware>();
app.Map("/login", async (MyUser user,DataContext db) => 
{
    List<Product> list = db.Products.ToList();
    MyUser? person = await db.MyUsers!.FirstOrDefaultAsync(p => p.Login == user.Login && p.Password == user.Password);
    if (person is null) return Results.Unauthorized();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
        var jwt = new JwtSecurityToken
        (
            issuer:AuthOptions.ISSUER,
            audience:AuthOptions.AUDIENCE,
            claims:claims,
            expires:DateTime.UtcNow.Add(TimeSpan.FromMinutes(5)),
            signingCredentials:new SigningCredentials(AuthOptions.GetSimmetricSecurutyKey(),SecurityAlgorithms.HmacSha256));
        var encoderJWT =new JwtSecurityTokenHandler().WriteToken(jwt);
        var response = new
        {
            access_token=encoderJWT,
            username = person.Login
        };
    return Results.Json(response);
    }
);
app.MapGet("/",() => "Hello World!");
var context = app.Services.CreateScope().ServiceProvider.
    GetRequiredService<DataContext>();
SeedData.SeedDatabase(context);
app.Run();

public class AuthOptions
{
    public const string ISSUER = "MyAuthServer";
    public const string AUDIENCE = "MyAuthClient";
    const string KEY = "mysupersecret_secretsecretkey!123";
    public static SymmetricSecurityKey GetSimmetricSecurutyKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}