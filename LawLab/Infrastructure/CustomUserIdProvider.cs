using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;

namespace LawLab.Infrastructure
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            ISession session = connection.GetHttpContext().Session;
            long studentId = session.Get<long>("student");
            long clientId = session.Get<long>("client");

            if (studentId != default)
            {
                if (connection.User.IsInRole("student"))
                {
                    return $"student{studentId}";
                }
            }
            else if (clientId != default)
            {
                if (connection.User.IsInRole("client"))
                {
                    return $"client{clientId}";
                }
            }

            return string.Empty;
        }
    }
}