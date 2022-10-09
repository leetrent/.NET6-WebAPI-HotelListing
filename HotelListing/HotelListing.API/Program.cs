using HotelListing.Data.Config;
using HotelListing.Data.Repositories;
using HotelListing.Data.Repositories.Interfaces;
using HotelListing.Identity.Config;
using HotelListing.Identity.Entities;
using HotelListing.Identity.Managers;
using HotelListing.Services;
using HotelListing.Services.Config;
using HotelListing.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");
string identittyConnectionString = builder.Configuration.GetConnectionString("HotelListingIdentityDbConnectionString");

// DOMAIN DB CONTEXT
builder.Services.AddDbContext<HotelListingDBContext>(options => {
    options.UseSqlServer(connectionString);
});

// IDENTITY DB CONTEXT
builder.Services.AddDbContext<HotelListingIdentityDBContext>(options => {
    options.UseSqlServer(identittyConnectionString);
});

// ADD REPOSITORIES TO THE CONTAINER
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();


// ADD SERVICES TO THE CONTAINER
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IHotelsService, HotelsService>();

// ADD MANAGERS TO THE CONTAINER
builder.Services.AddScoped<IAuthManager, AuthManager>();

// ADD AUTO-MAPPER CONFIGURATIONS TO the CONTAINER
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddAutoMapper(typeof(IdentityAutoMapperConfig));

// ADD IDENTITY CORE
builder.Services.AddIdentityCore<ApiUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<HotelListingIdentityDBContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

// (ctx) = HostBuilderContext; (lc) = LoggerConfiguration
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
