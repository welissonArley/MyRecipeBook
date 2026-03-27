using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using MyRecipeBook.Api.Filters;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo> { new("en"), new("pt-BR"), new("es") };

    options.DefaultRequestCulture = new RequestCulture("en");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = [ new AcceptLanguageHeaderRequestCultureProvider() ];
});

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();