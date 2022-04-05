using Microsoft.AspNetCore.SignalR;

namespace SignalRDemo.Models
{
   // [HubMethodName("notification")]
    public class NotificationHub: Hub
    {
    }
    public class Article
    {
        public string articleHeading { get; set; }
        public string articleContent { get; set; }
        public string userId { get; set; }
    }
}
