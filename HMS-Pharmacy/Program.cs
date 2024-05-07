using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


string hostname = "localhost";
string queueName = "drug_prescription";

var factory = new ConnectionFactory() { HostName = hostname };

using (var connection = factory.CreateConnection()) 
{
    using (var channel = connection.CreateModel())
    {
        channel.QueueDeclare(queue: queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        // Create a consumer
        var consumer = new EventingBasicConsumer(channel);

        // Event handler for received messages
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = System.Text.Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");
        };

        // Start consuming messages
        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        Console.ReadLine();
    }
}