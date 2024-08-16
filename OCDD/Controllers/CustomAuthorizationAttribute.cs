
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
/*
 * Dakoda Meade
 * CST-350
 * 
 * Custom autorize attribute filter
 * 
 */
namespace OCDD.Controllers
{
	public class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			string userID = context.HttpContext.Session.GetString("userID");

			if (userID == null)
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