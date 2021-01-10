using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Session;
using LawLab.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LawLab.Infrastructure.Filters
{
    public class ActiveUsersAttribute : Attribute, IAuthorizationFilter
    {
        //AuthorizationFilterContext context;
        //public ActiveUsersAttribute(AuthorizationFilterContext ctx)
        //{
        //    context = ctx;
        //}
        //public ActiveUsersAttribute(IServiceProvider sp)
        //{
        //    var session = sp.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
        //}        



        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var value = GetInstanceField(typeof(DistributedSession), context.HttpContext.Session, "_idleTimeout");
            //ISession session = context.HttpContext.Session;
            //if (context.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    session.SetString("student", context.HttpContext.User.Identity.Name);
            //}

            //if (context.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    Dictionary<string, DateTime> loggedInUsers = SecurityHelper.LoggedInUsers;

            //    if (context.HttpContext.User.Identity.IsAuthenticated)
            //    {
            //        if (loggedInUsers.ContainsKey(context.HttpContext.User.Identity.Name))
            //        {
            //            loggedInUsers[context.HttpContext.User.Identity.Name] = System.DateTime.Now;
            //        }
            //        else
            //        {
            //            loggedInUsers.Add(context.HttpContext.User.Identity.Name, System.DateTime.Now);
            //        }

            //    }

            //    // remove users where time exceeds session timeout
            //    var keys = loggedInUsers.Where(u => DateTime.Now.Subtract(u.Value).Minutes >
            //               context.HttpContext.Session.Timeout).Select(u => u.Key);
            //    foreach (var key in keys)
            //    {
            //        loggedInUsers.Remove(key);
            //    }
            //}
        }

        //internal static object GetInstanceField(Type type, object instance, string fieldName)
        //{
        //    BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        //        | BindingFlags.Static;
        //    FieldInfo field = type.GetField(fieldName, bindFlags);
        //    return field.GetValue(instance);
        //}
    }
}
