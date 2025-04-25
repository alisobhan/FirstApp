using Ardalis.Result;
using FirstApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FirstApp.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager,
                              IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
                              IAuthorizationService authorizationService) : IIdentityService
{
  private readonly UserManager<ApplicationUser> _userManager = userManager;
  private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
  private readonly IAuthorizationService _authorizationService = authorizationService;

  public async Task<bool> AuthorizeAsync(string userId, string policyName)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
    {
      return false;
    }

    var pricipal = await _userClaimsPrincipalFactory.CreateAsync(user);
    var result = await _authorizationService.AuthorizeAsync(pricipal, policyName);

    return result.Succeeded;
  }

  public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
  {
    throw new NotImplementedException();
  }

  public async Task<Result> DeleteUserAsync(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    return user != null ? await DeleteUserAsync(user) : Result.Success(); 
  }

  public async Task<Result> DeleteUserAsync(ApplicationUser user)
  {
    var result = await _userManager.DeleteAsync(user);

    return result.Succeeded
            ? Result.Success()
            : Result.Error(result.Errors.First().Description);
  }

  public async Task<string?> GetUserNameAsync(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    return user?.UserName;
  }

  public Task<bool> IsInRoleAsync(string userId, string role)
  {
    throw new NotImplementedException();
  }
}
