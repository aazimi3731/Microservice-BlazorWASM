using Azure.Messaging.ServiceBus;

namespace PaymentService.Services
{
  public class BusReceiverService
  {
    public static async Task<Tuple<ServiceBusClient, ServiceBusProcessor>> Initialize()
    {
      var azureConnectionString = WebApplication.CreateBuilder().Configuration["AzureQueues:AzureConnectionString"] ??
        throw new InvalidOperationException("Connection string 'AzureConnectionString' not found.");

      var azureQueueName = WebApplication.CreateBuilder().Configuration["AzureQueues:AzureQueueName"] ??
        throw new InvalidOperationException("Connection string 'AzureQueueName' not found.");

      // Set the transport type to AmqpWebSockets so that the ServiceBusClient uses the port 443. 
      // If you use the default AmqpTcp, ensure that ports 5671 and 5672 are open.
      var clientOptions = new ServiceBusClientOptions
      {
        TransportType = ServiceBusTransportType.AmqpWebSockets
      };

      // the client that owns the connection and can be used to create senders and receivers
      //
      // The Service Bus client types are safe to cache and use as a singleton for the lifetime
      // of the application, which is best practice when messages are being published or read
      // regularly.
      var client = new ServiceBusClient(azureConnectionString, clientOptions);


      // the processor that reads and processes messages from the queue
      var processor = client.CreateProcessor(azureQueueName, new ServiceBusProcessorOptions());

      try
      {
        // add handler to process messages
        processor.ProcessMessageAsync += MessageHandler;

        // add handler to process any errors
        processor.ProcessErrorAsync += ErrorHandler;

        // start processing 
        await processor.StartProcessingAsync();

        return Tuple.Create(client, processor);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message);
      }
    }

    // handle received messages
    private static async Task MessageHandler(ProcessMessageEventArgs args)
    {
      string body = args.Message.Body.ToString();
      Console.WriteLine($"Received: {body}");

      // complete the message. message is deleted from the queue. 
      await args.CompleteMessageAsync(args.Message);
    }

    // handle any errors when receiving messages
    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
      Console.WriteLine(args.Exception.ToString());
      return Task.CompletedTask;
    }
  }
}
