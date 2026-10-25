using School.Identity.Business.DependancyManager;
using School.Identity.Data.DependancyManager;
using School.Infrastructure.Extension;
using School.ViewModel.Common;

var builder = WebApplication.CreateBuilder(args);

JwtConfigurationsModel jwtConfigurationsModel
    = builder.Configuration.GetSection("JwtConfigurations").Get<JwtConfigurationsModel>() ?? new();
builder.Services.AddSingleton(jwtConfigurationsModel);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services
    .RegisterBusinessServices()
    .RegisterRepositoryServices()
    .AddDapper(builder.Configuration, "SchoolConnection");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
