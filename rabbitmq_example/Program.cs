// Create a connection factory

using System.Text;
using rabbitmq_example.Services;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    Port = 5672
};

RabbitMqExchanges exchanges = new RabbitMqExchanges(factory);

string? option;
do
{
    Console.WriteLine("Choose an option to test:");
    Console.WriteLine("1- Direct");
    Console.WriteLine("2- Fanout");
    Console.WriteLine("3- Topic");
    Console.WriteLine("4- Headers");
    
    option = Console.ReadLine();
  
    switch (option)
    {
        case "1":
            exchanges.Direct();
            break;
        case "2":
            exchanges.Fanout();
            break;
        case "3":
            exchanges.Topic();
            break;
        case "4":
            exchanges.Header();
            break;
    }

    Console.ReadLine();
    Console.Clear();
} while (option != "exit");