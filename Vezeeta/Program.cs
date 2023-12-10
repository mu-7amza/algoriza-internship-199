using Application.Services;
using Core.Abstractions;
using Core.Entities;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repositeries;
using Infrastructure.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Service.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services.Authentiction_Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.Configure<JWT>(builder.Configuration.GetSection("Authentication:JWT"));

#region add swagger options

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("vezeeta", new OpenApiInfo
    {
        Title = "vezeeta API",
        Version = "v3",
        Description = "Web API Project"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },

                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new string[]{}
        }
    });
});
#endregion

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();



// Add Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


// Add Application Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
// Add Authentication Services , Google , Facebook ,Jwt
#region authentication options 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Authentication:JWT:key").Value);
o.RequireHttpsMetadata = false;
o.SaveToken = false;
o.TokenValidationParameters = new TokenValidationParameters
{
    ValidateAudience = false,
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateLifetime = true,
    ValidIssuer = builder.Configuration["Authentication:JWT:Issuer"],
    ValidAudience = builder.Configuration["Authentication:JWT:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(key)
    };
})
    .AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("Authentication:Google:ClientId").Get<string>();
    googleOptions.ClientSecret = builder.Configuration.GetSection("Authentication:Google:ClientSecret").Get<string>();
})
    .AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = builder.Configuration.GetSection("Authentication:Facebook:AppId").Get<string>();
    facebookOptions.AppSecret = builder.Configuration.GetSection("Authentication:Facebook:AppSecret").Get<string>();
});

#endregion



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/vezeeta/swagger.json", "Vezeeta API"));
}

//app.UseCors(config =>
//{
//    config
//    .AllowAnyOrigin()
//    .AllowAnyMethod()
//    .AllowAnyHeader();
//});

// Configure the HTTP request pipeline.
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
