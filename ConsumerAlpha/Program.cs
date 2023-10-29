var hostName = "localhost";
var exchanegeName = "ProjectOne";


var factory = new ConnectionFactory() { HostName = hostName };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: exchanegeName, type:
        ExchangeType.Direct);
    var queueName = channel.QueueDeclare().QueueName;

    RoutingKey key1 = RoutingKey.Alpha;
    channel.QueueBind(queue: queueName, exchange: exchanegeName,
        routingKey: key1.GetDescription<RoutingKey>());

    RoutingKey key2 = RoutingKey.Beta;
    channel.QueueBind(queue: queueName, exchange: exchanegeName,
        routingKey: key2.GetDescription<RoutingKey>());

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
        var routingKey = ea.RoutingKey;
        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
    };

    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

