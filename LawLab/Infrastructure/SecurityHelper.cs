using System.Collections.Generic;

namespace LawLab.Infrastructure
{
    public static class SecurityHelper
    {
        public static List<long> LoggedInStudents { get; set; } = new List<long>();
        public static List<long> LoggedInClients { get; set; } = new List<long>();
    }
}
