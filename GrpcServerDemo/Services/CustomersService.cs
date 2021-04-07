using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcServerDemo.Services
{
    public class CustomersService : Customers.CustomersBase
    {
        public CustomersService()
        {

        }
        public override async Task GetCustomers(EmptyModel request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            var customers = new List<CustomerModel> {
             new CustomerModel{Email= "Ike.ajumobi@mail.com", Name="Ike"},
             new CustomerModel{Email= "Ola.ajumobi@mail.com", Name="Ola"},
             new CustomerModel{Email= "Ife.ajumobi@mail.com", Name="Ife"},
            };
            foreach(var customer in customers)
            {
                await responseStream.WriteAsync(customer);
            }
        }
    }
}
