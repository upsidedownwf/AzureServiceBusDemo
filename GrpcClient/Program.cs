using Grpc.Core;
using Grpc.Net.Client;
using GrpcServerDemo;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int x = 0;
            Console.WriteLine("Hello World!");
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var greeterClient = new Greeter.GreeterClient(channel);
            var customerClient = new Customers.CustomersClient(channel);
            var reply= await greeterClient.SayHelloAsync(new HelloRequest { Name = "Jon-Ajumobi Olamide D." });
            Console.WriteLine($"Message is: {reply.Message}");

            using (var customer = customerClient.GetCustomers(new EmptyModel()))
            {
                while(await customer.ResponseStream.MoveNext())
                {
                    var current = customer.ResponseStream.Current;
                    Console.WriteLine($"{++x}. Customer Name: {current.Name}, Customer Email: {current.Email}");
                }
            }
            Console.ReadLine();
        }
    }
}
