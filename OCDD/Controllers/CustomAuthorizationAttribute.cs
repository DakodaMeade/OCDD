
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
/*
 * Dakoda Meade
 * CustomAuthorizationAttribute
 * Custom autorize attribute filter
 * This checks is the user is logged in
 */
namespace OCDD.Controllers
{
	public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			string userID = context.HttpContext.Session.GetString("userID");
			// is the user logged in
			if (userID == null)
			{
				context.Result = new RedirectResult("/login"); // redirect to login page
			}
			else
			{
				// do nothing and let session proceed
			}
		}
	}
}