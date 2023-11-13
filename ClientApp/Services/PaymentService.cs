using Azure.Messaging.ServiceBus;

namespace ClientApp.Services
{
  public class PaymentServiceImpl : IPaymentService
  {
    // number of messages to be sent to the queue
    const int numOfMessages = 3;

    private static string? _azureConnectionString;

    private static string? _azureQueueName;

    public static void SetStringParameters(string azureConnectionString_, string azureQueueName_)
    {
      _azureQueueName = azureQueueName_;
      _azureConnectionString = azureConnectionString_;
    }

    public async Task Send(string message_)
    {
      // Set the transport type to AmqpWebSockets so that the ServiceBusClient uses the port 443. 
      // If you use the default AmqpTcp, ensure that ports 5671 and 5672 are open.
      var clientOptions = new ServiceBusClientOptions
      {
        TransportType = ServiceBusTransportType.AmqpWebSockets
      };

      // name of your Service Bus queue
      // the client that owns the connection and can be used to create senders and receivers
      //
      // The Service Bus client types are safe to cache and use as a singleton for the lifetime
      // of the application, which is best practice when messages are being published or read
      // regularly.
      var client = new ServiceBusClient(_azureConnectionString, clientOptions);

      // the sender used to publish messages to the queue
      var sender = client.CreateSender(_azureQueueName);

      // create a batch 
      using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

      // try adding a message to the batch
      if (!messageBatch.TryAddMessage(new ServiceBusMessage(message_)))
      {
        // if it is too large for the batch
        throw new Exception($"The message {message_} is too large to fit in the batch.");
      }

      try
      {
        // Use the producer client to send the batch of messages to the Service Bus queue
        await sender.SendMessagesAsync(messageBatch);
        Console.WriteLine($"A batch of one message has been published to the queue.");
      }
      finally
      {
        // Calling DisposeAsync on client types is required to ensure that network
        // resources and other unmanaged objects are properly cleaned up.
        await sender.DisposeAsync();
        await client.DisposeAsync();
      }

      Console.WriteLine("Press any key to end the application");
    }
  }
}
