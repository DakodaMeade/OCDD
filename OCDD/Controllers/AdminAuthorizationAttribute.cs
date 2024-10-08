﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
// Dakada Meade
// Class for veriftying if an admin user logged in
namespace OCDD.Controllers
{
    public class AdminAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// Checks if the user is logged in and an admin.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the user is logged in
            string userID = context.HttpContext.Session.GetString("userID");

            // Check if the user has the "admin" role
            string role = context.HttpContext.Session.GetString("userRole");

            if (userID == null || role != "Admin")
            {
                // Redirect to a "not authorized" page or login page
                context.Result = new RedirectResult("/");
            }
            else
            {
                // Do nothing and let the request proceed
            }
        }
    }
}
