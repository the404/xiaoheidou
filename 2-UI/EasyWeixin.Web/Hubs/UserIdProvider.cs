using Microsoft.AspNet.SignalR;
using System;

namespace EasyWeixin.Web.Hubs
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var userId = request.QueryString["groupId"];
            if (string.IsNullOrEmpty(userId))
                throw (new Exception("客户端未能提供 groupId"));
            return userId;
        }
    }
}