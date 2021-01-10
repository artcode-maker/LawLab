using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LawLab.Infrastructure
{
    public static class SecurityHelper
    {
        public static List<long> LoggedInStudents { get; set; } = new List<long>();
        public static List<long> LoggedInClients { get; set; } = new List<long>();

        //public static Dictionary<string, int> LoggedInUsers { get; set; } = new Dictionary<string, int>();

        //public static Dictionary<string, DateTime> GetLoggedInUsers(AuthorizationFilterContext httpContext)
        //{
        //    Dictionary<string, DateTime> loggedInUsers = new Dictionary<string, DateTime>();

        //    if (httpContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        loggedInUsers = (Dictionary<string, DateTime>)HttpContext.Current.Application["loggedinusers"];
        //        if (loggedInUsers == null)
        //        {
        //            loggedInUsers = new Dictionary<string, DateTime>();
        //            HttpContext.Current.Application["loggedinusers"] = loggedInUsers;
        //        }
        //    }
        //    return loggedInUsers;

        //}
    }
}
