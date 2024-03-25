using IdentityServer4.Services;
using InDuckTor.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using InDuckTor.Auth.Infrastructure.Database;
using InDuckTor.Auth.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();

// DB
builder.Services.AddAuthDbContext(configuration);
builder.Services.AddIdentityDbContext(configuration);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

// Identity Server
var rsaKey = RSA.Create();
string xmlKey = File.ReadAllText(configuration.GetSection("Jwt:PrivateKeyPath").Value);
rsaKey.FromXmlString(xmlKey);
var securityKey = new RsaSecurityKey(rsaKey);
var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

builder.Services.AddIdentityServer(option =>
{
    option.IssuerUri = "in-duck-tor";
    option.AccessTokenJwtType = "JWT";
})
    .AddSigningCredential(signingCred)
    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
    .AddInMemoryClients(IdentityServerConfig.GetClients());

builder.Services.AddTransient<IProfileService, IdentityClaimsProfileService>();

// HTTP client
builder.Services.AddHttpClient<IUserHttpClient, UserHttpClient>();

// Services
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICredentialsService, CredentialsService>();

// CORS
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.Run();
