
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OCDD.Models;
using OCDD.Services.Utility;

/*
 * Dakoda Meade
 * log action filter
 * 
 */
namespace OCDD.Controllers
{
	public class LogActionFilterAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
			UserModel user = (UserModel)((Controller)context.Controller).ViewData.Model;
			MyLogger.GetInstance().Info("Parameter: " + user.ToString());
			MyLogger.GetInstance().Info("Leaving the ProcessLogin Method");
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			MyLogger.GetInstance().Info("Entering the ProcessLogin Method");
		}
	}
}