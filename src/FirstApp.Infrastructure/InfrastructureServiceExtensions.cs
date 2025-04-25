using FirstApp.Core.Constants;
using FirstApp.Core.Interfaces;
using FirstApp.Core.Services;
using FirstApp.Infrastructure.Data;
using FirstApp.Infrastructure.Data.Queries;
using FirstApp.Infrastructure.Identity;
using FirstApp.UseCases.Contributors.List;
using Microsoft.AspNetCore.Identity;


namespace FirstApp.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
           .AddScoped<IDeleteContributorService, DeleteContributorService>();

    services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();


    services.AddTransient<IIdentityService, IdentityService>();
    services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
