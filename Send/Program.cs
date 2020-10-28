using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
  class Program
  {
    private static string url = "amqps://vqrdeoyt:yb6lhx1bhEqTApVA701LbjVXjiGbGW1N@jackal.rmq.cloudamqp.com/vqrdeoyt";
    static readonly ConnectionFactory connFactory = new ConnectionFactory
    {
    };

    static void Main(string[] args)
    {
        using (var conn = connFactory.CreateConnection())
        {
            using (var channel = conn.CreateModel())
            {
                channel.QueueDeclare(queue: "fila-teste-01",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
                
                for (int i = 1; i <= 10; i++)
                {
                    // The message we want to put on the queue
                    var message = i.ToString() + ") " + DateTime.Now.ToString("F");
                    
                    // the data put on the queue must be a byte array
                    var data = Encoding.UTF8.GetBytes(message);
                    
                    // publish to the "default exchange", with the queue name as the routing key
                    var exchangeName = "";
                    var routingKey = "fila-teste-01";
                    channel.BasicPublish(exchangeName, routingKey, null, data);

                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }

    /*
    static void Main(string[] args)
    {
      Console.WriteLine("Send Program");

      var factory = new ConnectionFactory() { HostName = "localhost" };
      using (var connection = factory.CreateConnection())
      {
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "fila-teste-01",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "fila-teste-01",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" - Enviar: {0}", message);
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
      }
    }
    */
  }
}
