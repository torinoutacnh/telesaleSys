//using Microsoft.AspNetCore.SignalR;
//using System.Threading.Tasks;

//namespace XProject.WebApi.Hubs
//{
//    public class ChatHub : Hub
//    {
//        public async Task SendAsync(string user, string message)
//        {
//            await Clients.All.SendAsync("ReceiveMessage", user, message);
//        }

//        public void Send(string name, string message)
//        {
//            // Call the broadcastMessage method to update clients.
//            Clients.All.SendAsync(name, message);
//        }
//    }
//}
