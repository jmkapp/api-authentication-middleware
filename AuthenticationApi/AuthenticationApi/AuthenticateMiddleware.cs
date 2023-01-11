using AuthenticationApi.Services;

namespace AuthenticationApi
{
    public class AuthenticateMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers.TryGetValue("username", out var userName);
            context.Request.Headers.TryGetValue("password", out var password);

            if (userName.FirstOrDefault() == null || password.FirstOrDefault() == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized Access");
                return;
            }

            IUserService userService = context.RequestServices.GetRequiredService<IUserService>();

            var authenticated = await userService.VerifyPassword(userName.First(), password.First());

            if (!authenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized Access");
                return;
            }

            await _next(context);
        }
    }
}
