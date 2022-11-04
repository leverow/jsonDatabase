using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Task11.Entity;
using Task11.Services;

namespace Task11.Attributes;

public class AuthAttribute : ActionFilterAttribute
{
    private readonly IUserService _userService;

    public AuthAttribute(IUserService userService)
    {
        _userService = userService;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if(!context.HttpContext.Request.Headers.ContainsKey("Key"))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var key = context.HttpContext.Request.Headers["Key"];
        var user = _userService.RetrieveUserByKey(key.ToString());
        if (user is null)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
