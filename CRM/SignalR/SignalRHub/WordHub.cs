
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

public class WordHub : Hub
{
    public async Task SendMessage(string message, string name)
    {
        await Clients.All.SendAsync("ReceiveMessage", message, name);

    }//sadaasasdфывфывфывфыв
}


