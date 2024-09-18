using Main.Infrastructure;
using Share;
using Share.EmailService;
using Share.FileManagement;
using Main.Infrastructure.Authentications;
using Microsoft.OpenApi.Models;
using CrystalFresh.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var deploymentConfigData = new DeploymentConfiguration
{
    LogoUrl = Environment.GetEnvironmentVariable("LOGO_URL") ?? "fuf",
    SupportTeamEmail = Environment.GetEnvironmentVariable("SUPPORT_TEAM_EMAIL") ?? "supportteam@swinva.com",
};

var objStorageConfigData = new ObjectStorageConfiguration
{
    SecretKey = Environment.GetEnvironmentVariable("CLOUDINARY_SECRET_ACCESS_KEY") ?? "rytry",
    AccessKey = Environment.GetEnvironmentVariable("CLOUDINARY_ACCESS_KEY") ?? "tutru",
    StorageName = Environment.GetEnvironmentVariable("CLOUDINARY_NAME") ?? "ytryry"
};
var emailConfigData = new EmailConfiguration
{
    Host = Environment.GetEnvironmentVariable("EMAIL_HOST") ?? "smtp.ethereal.email",
    Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT") ?? "587"),
    Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? "r4E5MXfZysGWf4pqya",
    Sender = Environment.GetEnvironmentVariable("EMAIL_SENDER") ?? "zackary.oconnell@ethereal.email"
};

var mainConfigData = new MainConfigData
{
    AUTH_SECRET_KEY = Environment.GetEnvironmentVariable("AUTH_SECRET_KEY") ?? "e5l848r64w,krruiewh9hgu94gjuu2.ttvte468d",
    MAIN_DB_CONNECT_STRING = Environment.GetEnvironmentVariable("MAIN_DB_CONNECT_STRING") ?? "Host=localhost;Port=5432;Database=Crystal_auth1db;Username=postgres;Password=admin;"
};

builder.Services.AddShareServices(opts =>
{
    opts.DEPLOYMENT_CONFIGURATION = deploymentConfigData;
    opts.ObjectStorageConfiguration = objStorageConfigData;
    opts.EMAIL_CONFIGURATION = emailConfigData;
});
string SpecificOrigin = "AllowSpecificOrigin";
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(po =>
    {
        po.AllowAnyOrigin()
            .AllowAnyHeader()
            .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE");
    });
});

//var loggerConf = new LoggerConfiguration()
//    .Enrich.FromLogContext()
//    .WriteTo.Seq("http://seq")
//    .CreateLogger();

//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(loggerConf);
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
builder.Services.AddMainServices(opts =>
{
    opts.MAIN_DB_CONNECT_STRING=mainConfigData.MAIN_DB_CONNECT_STRING;
    opts.AUTH_SECRET_KEY=mainConfigData.AUTH_SECRET_KEY;
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crystal Fresh API V1");
    if (!app.Environment.IsDevelopment())
        c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();