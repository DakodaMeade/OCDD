
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
/*
 * Dakoda Meade
 * CST-350
 * Activity 6-2
 * Custom autorize attribute filter
 * 
 */
namespace RegisterAndLoginApp.Controllers
{
    public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string username = context.HttpContext.Session.GetString("username");

            if (username == null)
            {
                context.Result = new RedirectResult("/login");
            }
            else 
            {
                // do nothing and let session proceed
            }
        }
    }
}