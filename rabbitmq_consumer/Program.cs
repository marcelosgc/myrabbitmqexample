// See https://aka.ms/new-console-template for more information

// Your worker logic here...

// Create a connection factory

using System.Text;
using rabbitmq_consumer.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest",
    Port = 5672
};

RabbitMqConsumer rabbitMqConsumer = new RabbitMqConsumer(factory);
rabbitMqConsumer.GenerateDirectConsumers("direct_exchange", "direct_queue", "direct_queue");
rabbitMqConsumer.GenerateFanoutConsumers("fanout_exchange", "fanout_queue", string.Empty);
rabbitMqConsumer.GenerateTopicConsumers("topic_exchange", "topic_queue", "topic.*");
rabbitMqConsumer.GenerateHeaderConsumers("header_exchange", "header_queue", "");
rabbitMqConsumer.GenerateHeaderConsumers2("header_exchange", "header_queue", "");

Console.ReadLine();