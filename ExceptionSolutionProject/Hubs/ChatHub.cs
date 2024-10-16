// SignalR Hub metodu
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string userId, string message)
    {
        // Belirtilen kullanıcıya mesajı ilet
        await Clients.User(userId).SendAsync("ReceiveMessage", message);
    }
}
