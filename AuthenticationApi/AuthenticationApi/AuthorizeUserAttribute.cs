using AuthenticationApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthenticationApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeUserAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Request.Headers.TryGetValue("username", out var userName);
            context.HttpContext.Request.Headers.TryGetValue("password", out var password);

            if (userName.FirstOrDefault() == null || password.FirstOrDefault() == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            var authenticated = await userService.VerifyPassword(userName.First(), password.First());

            if (!authenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
