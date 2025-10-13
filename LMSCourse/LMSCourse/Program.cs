using FluentValidation;
using FluentValidation.AspNetCore;
using LMSCourse.Data;
using LMSCourse.DTOs.ZaloPay;
using LMSCourse.Extensions;
using LMSCourse.Interfaces;
using LMSCourse.Mapper;
using LMSCourse.Models;
using LMSCourse.Repositories;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Environment.WebRootPath ??= Path.Combine(builder.Environment.ContentRootPath, "wwwroot");

// Add services to the container.

builder.Services.AddControllers();
// Đăng ký FluentValidation (cách mới)
builder.Services
    .AddFluentValidationAutoValidation()       // Tự động validate khi ModelState
    .AddFluentValidationClientsideAdapters();  // Nếu dùng MVC view, hỗ trợ client-side

// Scan tất cả Validator trong assembly hiện tại
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineCoursed API", Version = "v1" });

    // Add JWT Bearer Authorization
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token in this format: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200") // Angular chạy ở 4200
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});





//Thêm DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnSQL"))
);

//Thêm DI

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddServicesAndRepositorys();

// ZaloPayOptions
builder.Services.Configure<ZaloPayOptions>(
    builder.Configuration.GetSection("ZaloPay"));

//JWT
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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.AddAutoMapper(typeof(AppProfile));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseMiddleware<AuditLogMiddleware>();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbInitializer.InitializeAsync(context);

    var permissionRepo = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();
    var permissions = await permissionRepo.GetAllAsync();

    var authOptions = scope.ServiceProvider
    .GetRequiredService<IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions>>()
    .Value;
    foreach (var perm in permissions)
    {
        authOptions.AddPolicy(perm.PermissionCode, policy =>
            policy.RequireClaim("Permission", perm.PermissionCode));
    }
}

app.Run();
