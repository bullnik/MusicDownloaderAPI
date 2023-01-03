using Microsoft.AspNetCore.Connections;
using MusicDownloaderAPI.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.ObjectModel;
using System.Text;

namespace MusicDownloaderAPI.Rabbit
{
    //public class RabbitMqService : IRabbitMqService
    //{
    //    private IConnection _connection;
    //    private IModel _channel;
    //    private string _requestQueueName = "MusicDownloadRequests";

    //    public RabbitMqService()
    //    {
    //        Console.WriteLine("CREATING CONNECTION");
    //        var factory = GetConnectionFactory();
    //        _connection = factory.CreateConnection();
    //        Console.WriteLine("CONNECTION CREATED");
    //        _channel = _connection.CreateModel();
    //        _channel.QueueDeclare(queue: _requestQueueName, durable: false,
    //            exclusive: false, autoDelete: false, arguments: null);
    //        var consumer = new EventingBasicConsumer(_channel);
    //        consumer.Received += (model, ea) =>
    //        {
    //            var body = ea.Body.ToArray();
    //            var message = Encoding.UTF8.GetString(body);
    //            Console.WriteLine("RECEIVED MESSAGE " + message);
    //            Console.WriteLine("START DOWNLOADING " + message);
    //            Downloader downloader = new();
    //            downloader.Download(message);
    //            Console.WriteLine("END DOWNLOADING " + message);
    //        };
    //        _channel.BasicConsume(queue: _requestQueueName,
    //                             autoAck: true,
    //                             consumer: consumer);
    //    }

    //    private ConnectionFactory GetConnectionFactory()
    //    {
    //        return new ConnectionFactory
    //        {
    //            UserName = "bykrabbit",
    //            Password = "bykbykbyk",
    //            VirtualHost = "/",
    //            HostName = "rabbit",
    //            Port = AmqpTcpEndpoint.UseDefaultPort
    //        };
    //    }
    //}
}
