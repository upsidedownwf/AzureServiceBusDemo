using System.Linq;
using System.Threading.Tasks;

namespace AzureServiceBusDemoWebAPI.Models
{
    public class User
    {
        public string USERID { get; set; }
        public string PASSWORD { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string EMAILID { get; set; }
        public string PHONE { get; set; }
        public string ACCESS_LEVEL { get; set; }
        public string READ_ONLY { get; set; }
    }

}
