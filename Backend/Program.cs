using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Interfaces;
using Backend.Repository;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDBContext>(); // Add Identity to the project

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =  // Default scheme that will handle the authentication
    options.DefaultChallengeScheme =    // Default scheme that will handle the challenge
    options.DefaultForbidScheme =  // Default scheme that will handle the forbid
    options.DefaultScheme =  // Default scheme that will handle the authentication and challenge
    options.DefaultSignInScheme =   // Default scheme that will handle the sign in
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;   // Default scheme that will handle the sign out
}).AddJwtBearer( options =>  // Add JWT bearer authentication
{
    options.TokenValidationParameters = new TokenValidationParameters   // Set the parameters for the token validation
    {
        ValidateIssuer = true, // Validate the server that created that token
        ValidateAudience = true, // Ensure that the recipient of the token is authorized to receive it
        ValidateLifetime = true, // Check that the token is not expired and that the signing key of the issuer is valid
        ValidateIssuerSigningKey = true,  // Verify that the key used to sign the incoming token is part of a list of trusted keys
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            )
    };
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
