using LMSCourse.Data;
using LMSCourse.Models;
using LMSCourse.Repositories;
using LMSCourse.Repositories.Interfaces;
using LMSCourse.Services;
using LMSCourse.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineCourseConstants;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineCourse API", Version = "v1" });

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

builder.Services.AddAuthorization(options =>
{
    // User
    options.AddPolicy(PERMISSION.ViewUsers, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewUsers));
    options.AddPolicy(PERMISSION.CreateUsers, policy =>
        policy.RequireClaim("Permission", PERMISSION.CreateUsers));
    options.AddPolicy(PERMISSION.EditUsers, policy =>
        policy.RequireClaim("Permission", PERMISSION.EditUsers));
    options.AddPolicy(PERMISSION.DeleteUsers, policy =>
        policy.RequireClaim("Permission", PERMISSION.DeleteUsers));

    // Roles
    options.AddPolicy(PERMISSION.ViewRoles, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewRoles));
    options.AddPolicy(PERMISSION.CreateRoles, policy =>
        policy.RequireClaim("Permission", PERMISSION.CreateRoles));
    options.AddPolicy(PERMISSION.EditRoles, policy =>
        policy.RequireClaim("Permission", PERMISSION.EditRoles));
    options.AddPolicy(PERMISSION.DeleteRoles, policy =>
        policy.RequireClaim("Permission", PERMISSION.DeleteRoles));

    // Courses
    options.AddPolicy(PERMISSION.ViewCourses, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewCourses));
    options.AddPolicy(PERMISSION.CreateCourses, policy =>
        policy.RequireClaim("Permission", PERMISSION.CreateCourses));
    options.AddPolicy(PERMISSION.EditCourses, policy =>
        policy.RequireClaim("Permission", PERMISSION.EditCourses));
    options.AddPolicy(PERMISSION.DeleteCourses, policy =>
        policy.RequireClaim("Permission", PERMISSION.DeleteCourses));

    // Lessons
    options.AddPolicy(PERMISSION.ViewLessons, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewLessons));
    options.AddPolicy(PERMISSION.CreateLessons, policy =>
        policy.RequireClaim("Permission", PERMISSION.CreateLessons));
    options.AddPolicy(PERMISSION.EditLessons, policy =>
        policy.RequireClaim("Permission", PERMISSION.EditLessons));
    options.AddPolicy(PERMISSION.DeleteLessons, policy =>
        policy.RequireClaim("Permission", PERMISSION.DeleteLessons));

    // Enrollments
    options.AddPolicy(PERMISSION.ViewEnrollments, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewEnrollments));
    options.AddPolicy(PERMISSION.ManageEnrollments, policy =>
        policy.RequireClaim("Permission", PERMISSION.ManageEnrollments));

    // Payments
    options.AddPolicy(PERMISSION.ViewPayments, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewPayments));
    options.AddPolicy(PERMISSION.ManagePayments, policy =>
        policy.RequireClaim("Permission", PERMISSION.ManagePayments));

    // Logs
    options.AddPolicy(PERMISSION.ViewLogs, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewLogs));

    // System PERMISSION
    options.AddPolicy(PERMISSION.ViewPermissions, policy =>
        policy.RequireClaim("Permission", PERMISSION.ViewPermissions));
    options.AddPolicy(PERMISSION.ManagePermissions, policy =>
        policy.RequireClaim("Permission", PERMISSION.ManagePermissions));
});


//Thêm DbContext (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnSQL"))
);

//Thêm DI
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<TokenService>();

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

builder.Services.AddAutoMapper(typeof(Program));

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
app.UseAuthorization();

app.MapControllers();

app.Run();
