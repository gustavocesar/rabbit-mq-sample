using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

namespace Receive
{
  class Program
  {
    static void Main(string[] args)
    {
      var factory = new ConnectionFactory() { HostName = "localhost" };
      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
        channel.QueueDeclare(queue: "fila-teste-01",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
          var body = ea.Body.ToArray();
          var message = Encoding.UTF8.GetString(body);
          Console.WriteLine(" - Recebido: {0}", message);
        };
        channel.BasicConsume(queue: "fila-teste-01",
                             autoAck: true,
                             consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
      }
    }
  }
}
