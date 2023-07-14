using System.Text;
using RabbitMQ.Client;

namespace rabbitmq_example.Services;

public class RabbitMqExchanges
{
    private readonly ConnectionFactory _factory;

    public RabbitMqExchanges(ConnectionFactory factory)
    {
        _factory = factory;
    }

    public void Direct()
    {
        // Create a connection and channel
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        // Declare a direct exchange
        channel.ExchangeDeclare(exchange: "direct_exchange", type: ExchangeType.Direct);

        // Publish a message with a specific routing key
        var message = "Direct exchange message";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "direct_exchange", routingKey: "direct_queue", basicProperties: null,
            body: body);
    }

    public void Fanout()
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange: "fanout_exchange", type: ExchangeType.Fanout);

        var message = "Fanout exchange message";
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "fanout_exchange", routingKey: "", basicProperties: null, body: body);
    }

    public void Topic()
    {
        var ttl = new Dictionary<string, object>
        {
            {"x-message-ttl", 30000}
        };

        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare("topic_exchange", ExchangeType.Topic, arguments: ttl);

            var message = "Topic exchange message";
            var body = Encoding.UTF8.GetBytes(message);
            var routingKey = "topic.*";

            channel.BasicPublish(exchange: "topic_exchange", routingKey: routingKey, basicProperties: null, body: body);
        }

        ;
    }

    public void Header()
    {
        using var connection = _factory.CreateConnection();
        using (var channel = connection.CreateModel())
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000}
            };

            channel.ExchangeDeclare(exchange: "header_exchange", type: ExchangeType.Headers, arguments: ttl);

            var message = "Headers exchange message";
            var body = Encoding.UTF8.GetBytes(message);
            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object>
            {
                {"header1", "value1"},
                {"header2", "value2"}
            };

            // Publish the message with the specified headers
            channel.BasicPublish(exchange: "header_exchange", routingKey: "", basicProperties: properties, body: body);
        }
    }
}