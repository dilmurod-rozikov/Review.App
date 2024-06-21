using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
if (args.Length == 1 && args[0].Equals("seeddata", StringComparison.CurrentCultureIgnoreCase))
{
    SeedData(app);
    return; // Exit after seeding data
}

static void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>() ?? throw new InvalidOperationException("IServiceScopeFactory is not registered in the service container.");
    using var scope = scopedFactory.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<Seed>();
    service.SeedDataContext();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();