using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace blockchainapi.Hubs
{
    public class BlockHub : Hub
    {
        public async Task Send()
        {
            await Clients.All.SendAsync("ReceiveData", "BlockHub Hit!");
        }
    }
}
