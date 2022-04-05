using System.Collections.Generic;

namespace AzureServiceBusDemoWebAPI.Models
{
    public interface ITokenProvider
    {
        List<User> userList { get; }
        string LoginUser(string UserID, string Password);
    }

}
