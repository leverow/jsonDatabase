using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using Task11.Entity;
using Task11.Services;

namespace Task11.Attributes;

public class AuthAttribute : IActionFilter
{
    private readonly IUserService _userService;

    public string Roles { get; set; }

    public AuthAttribute(IUserService userService, string roles)
    {
        _userService = userService;
        Roles = roles;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if(!context.HttpContext.Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var authorization = context.HttpContext.Request.Headers[HeaderNames.Authorization];

        var user = _userService.RetrieveUserByKey(authorization.ToString());
        if (user is null)
        {
            context.Result = new UnauthorizedResult();
        }
        
        if(!Roles.Contains(user!.Role.ToString()))
        {
            context.HttpContext.Response.StatusCode = 401;
            context.Result = new JsonResult(new { Error = "Access denied" });
            return;
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Name, user.Login)
        };

        var identity = new ClaimsIdentity(claims);

        var principal = new ClaimsPrincipal(identity);

        context.HttpContext.User = principal;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
