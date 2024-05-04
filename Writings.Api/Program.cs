using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using Writings.Api.Auth;
using Writings.Api.Mappings;
using Writings.Api.Swagger;
using Writings.Application.Data;
using Writings.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(AuthConstants.AdminUserPolicyName, p => 
        p.RequireClaim(AuthConstants.AdminUserClaimName, "true"));

    o.AddPolicy(AuthConstants.TrustedMemberPolicyName, 
        p => p.RequireAssertion(h => 
            h.User.HasClaim(m => m is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
            h.User.HasClaim(m => m is { Type: AuthConstants.TrustedMemberClaimName, Value: "true"})));
});

builder.Services.AddApiVersioning(o => 
{
    o.DefaultApiVersion = new Asp.Versioning.ApiVersion(1.0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
}).AddMvc().AddApiExplorer();


builder.Services.AddControllers();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddSwaggerGen(o => o.OperationFilter<SwaggerDefaultValues>());

builder.Services.AddApplication();

builder.Services.AddDatabase(builder.Configuration.GetConnectionString("WritingsContext")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(a =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            a.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();

app.MapControllers();

app.Run();
