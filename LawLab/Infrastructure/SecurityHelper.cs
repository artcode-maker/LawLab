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
    }
}
