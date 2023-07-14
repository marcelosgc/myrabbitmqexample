using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rabbitmq_consumer.Services;

public class RabbitMqConsumer
{
    private readonly ConnectionFactory _factory;

    public RabbitMqConsumer(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public async void GenerateDirectConsumers(string exchangeName, string queueName, string routingKey)
    {
        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            // Declare a direct exchange
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);


            channel.QueueDeclare(queueName, true, false, false);

            // Declare a queue and bind it to the direct exchange with a specific routing key
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);

            // Create a consumer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine("Direct exchange message received: {0}", message);
            };


            // Start consuming messages from the queue in a separate thread
            await Task.Run(() => channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer));
            // Keep the application running
            await Task.Delay(-1);
        }
    }

    public async void GenerateFanoutConsumers(string exchangeName, string queueName, string routingKey)
    {
        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            // Declare a direct exchange
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);


            channel.QueueDeclare(queueName, true, false, false);

            // Declare a queue and bind it to the direct exchange with a specific routing key
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey);

            // Create a consumer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine("Fanout exchange message received to tag {1}: {0}", message, ea.ConsumerTag);
            };
            // Start consuming messages from the queue in a separate thread
            await Task.Run(() => channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer));
            // Keep the application running
            await Task.Delay(-1);
        }
    }

    public async void GenerateTopicConsumers(string exchangeName, string queueName, string routingKey)
    {
        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(queueName, exchangeName, routingKey);


            // Create a consumer
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine("Topic exchange message received to tag {1}: {0}", message, ea.ConsumerTag);
            };
            // Start consuming messages from the queue in a separate thread
            await Task.Run(() => channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer));
            // Keep the application running
            await Task.Delay(-1);
        }
    }
    
    public async void GenerateHeaderConsumers(string exchangeName, string queueName, string routingKey)
    {
        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Headers);
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header = new Dictionary<string, object> {{"header1", "value1"}};
            
            channel.QueueBind(queueName, exchangeName, routingKey,header);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine("Header exchange message received to tag {1}: {0}", message, ea.ConsumerTag);
            };
            // Start consuming messages from the queue in a separate thread
            await Task.Run(() => channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer));
            // Keep the application running
            await Task.Delay(-1);
        }
    }
    
    public async void GenerateHeaderConsumers2(string exchangeName, string queueName, string routingKey)
    {
        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchangeName, ExchangeType.Headers);
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var header = new Dictionary<string, object> {{"header2", "value2"}};
            
            channel.QueueBind(queueName, exchangeName, routingKey,header);
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine("Header exchange message received to tag {1}: {0}", message, ea.ConsumerTag);
            };
            // Start consuming messages from the queue in a separate thread
            await Task.Run(() => channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer));
            // Keep the application running
            await Task.Delay(-1);
        }
    }
}