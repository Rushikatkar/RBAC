using BAL.Services;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Presentation_Layer.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add Swagger/OpenAPI for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context for Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCON"))
);

// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Allow requests without a token
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Skip validation if no token is provided
            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
            {
                context.NoResult();
            }
            return Task.CompletedTask;
        }
    };
});

// Register custom authorization handler
builder.Services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();

// Register authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.Requirements.Add(new RoleRequirement(1)));  // Admin role

    options.AddPolicy("EmployeeOnly", policy =>
        policy.Requirements.Add(new RoleRequirement(2)));  // Employee role

    options.AddPolicy("ManagerOnly", policy =>
        policy.Requirements.Add(new RoleRequirement(2)));  // Manager role

    options.AddPolicy("ManagerOrAdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Manager") || context.User.IsInRole("Admin") // Allow both Manager and Admin roles
        )
    );

    options.AddPolicy("EmployeeOrAdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Employee") || context.User.IsInRole("Admin") // Allow both Employee and Admin roles
        )
    );

});

// Add Swagger with JWT Authentication support
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token. Leave it blank for public APIs.",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure CORS to allow specific origins (frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Replace with your frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Authentication and Authorization Middleware
app.UseAuthentication(); // Ensure authentication middleware is in place
app.UseAuthorization();  // Ensure authorization middleware is in place

app.MapControllers(); // Map controllers to routes

app.Run();
