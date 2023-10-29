var hostName = "localhost";
var exchanegeName = "ProjectOne";

var factory = new ConnectionFactory() { HostName = hostName };

using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{

    channel.ExchangeDeclare(exchange: exchanegeName, type: ExchangeType.Direct);

    while (true)
    {
        Console.WriteLine("Write what you want to send");
        Console.WriteLine("Write nothing to exit.");

        string usermessage = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(usermessage))
            continue;

        Console.WriteLine("Choose your routingKey");
        Console.WriteLine("1. Alpha");
        Console.WriteLine("2. Beta");

        char userRoutingKey = Console.ReadKey().KeyChar;
        int userRoutingNumber = int.Parse(userRoutingKey.ToString());

        if (userRoutingNumber == 1 || userRoutingNumber == 2)
        {
            RoutingKey key = (RoutingKey)int.Parse(userRoutingKey.ToString());

            var body = Encoding.UTF8.GetBytes(usermessage);

            channel.BasicPublish(exchange: exchanegeName,
                routingKey: key.GetDescription<RoutingKey>(),
                basicProperties: null, body: body);

            ConsoleMessageHelper.WriteMessageOnConsole($"{usermessage}:{key.GetDescription<RoutingKey>()}");
        }
    }
}