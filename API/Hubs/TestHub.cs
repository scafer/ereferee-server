using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Hubs
{
    public class TestHub: Hub
    {
        public async void SendData(string data)
        {
            await Clients.All.SendAsync("ReceiveData", data);
        }
    }
}
