using Abstraction.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace WebApp.Filter
{
    public class AuthorizeAttribute : IAsyncActionFilter
    {
        private StringValues _authorizationToken;
        private IAuthService _jwtService;
        public AuthorizeAttribute(IAuthService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool token = context.HttpContext.Request.Headers.TryGetValue("Authorization", out _authorizationToken);
            if (token)
            {
                var isValidToken = _jwtService.ValidateToken(_authorizationToken.ToString().Substring("Bearer ".Length));
                if (!isValidToken)
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    await next.Invoke();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
