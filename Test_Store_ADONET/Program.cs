using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Threading.RateLimiting;
using Test_Store_ADONET.Database;
using Test_Store_ADONET.Repository.AppUser;
using Test_Store_ADONET.Repository.Brands;
using Test_Store_ADONET.Repository.Products;
using Test_Store_ADONET.Services.Brands;
using Test_Store_ADONET.Services.JWT;
using Test_Store_ADONET.Services.Login;
using Test_Store_ADONET.Services.Products;
using Test_Store_ADONET.Swagger;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region Serilog
var _logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());
builder.Logging.AddSerilog(_logger);
#endregion

#region Rate Limiting
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    #region Fixed by ip
    rateLimiterOptions.AddPolicy("fixed-by-ip", httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
        factory: _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(5)
        }));
    #endregion

    #region Token bucket jwt
    rateLimiterOptions.AddPolicy(policyName: "token-jwt", partitioner: httpContext =>
    {
        var accessToken = httpContext.Features.Get<IAuthenticateResultFeature>()?
            .AuthenticateResult?.Properties?.GetTokenValue("access_token")?.ToString()
            ?? string.Empty;

        return RateLimitPartition.GetTokenBucketLimiter(accessToken, _ =>
            new TokenBucketRateLimiterOptions
            {
                TokenLimit = 10, //50
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 5, //10
                ReplenishmentPeriod = TimeSpan.FromMinutes(5), //30
                TokensPerPeriod = 5, //20
                AutoReplenishment = true
            });
    });
    #endregion

    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});
#endregion

#region Database Connection
builder.Services.AddScoped<IDatabaseConnection, DatabaseConnection>();
#endregion

#region Repositories
builder.Services.AddScoped<IBrandsRepository, BrandsRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IAppUsersRepository, AppUsersRepository>();

#endregion

#region Services
builder.Services.AddScoped<IBrandsService, BrandsService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IJWTService, JWTService>();
#endregion

#region Configuración JWT
var jwtSection = builder.Configuration.GetSection("JWTSettings");

builder.Services.Configure<JWTSettings>(jwtSection);

var appSettings = jwtSection.Get<JWTSettings>();

var key = Encoding.UTF8.GetBytes(appSettings.SecretKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = appSettings.Issuer,
        ValidAudience = appSettings.Audience,
    };
});

#endregion

#region Swagger configuration
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();

    var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

    options.IncludeXmlComments(filePath);
});
#endregion

#region API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = ApiVersion.Default;
    options.ReportApiVersions = true;
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
})
.EnableApiVersionBinding();
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var descriptions = app.DescribeApiVersions();

    foreach (var description in descriptions)
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        var name = description.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name);
    }
});
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();