using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApplication1
{
    public class TestHub : Hub
    {
        public void Hello(string message = "Hello world")
        {
            Clients.All.hello(message);
            Clients.All.startAward(11);
        }

        public void Hello2(Test test)
        {
            Clients.All.hello(test.message);
        }

        public void Send(string openId)
        {
            //用来发送模板消息
        }
    }

    public class Test
    {
        public string message { get; set; }
    }
}