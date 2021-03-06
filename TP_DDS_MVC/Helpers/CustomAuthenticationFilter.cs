﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using TP_DDS_MVC.Models.Otros;

namespace TP_DDS_MVC.Helpers
{
    public class CustomAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["usuario"])))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else if(((Usuario)filterContext.HttpContext.Session["usuario"]).idEntidad == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "Entidad" },
                     { "action", "MenuCargarEntidad" }
                });
            }
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                //Redirecting the user to the Login View of Account Controller  
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                     { "controller", "User" },
                     { "action", "Login" }
                });
            }
            
        }
    }
}