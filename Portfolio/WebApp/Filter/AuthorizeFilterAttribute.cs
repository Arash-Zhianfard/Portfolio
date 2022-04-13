using Microsoft.AspNetCore.Mvc;

namespace WebApp.Filter
{
    public class AuthorizeFilterAttribute : TypeFilterAttribute
    {
        public AuthorizeFilterAttribute() : base(typeof(AuthorizeAttribute))
        {

        }
    }
}
