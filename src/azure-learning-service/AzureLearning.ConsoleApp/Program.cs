
using TShared.Azure.ServiceBus;

var messageService = new MessageService();
var sendTask = messageService.SendMessageAsync();
var receiveTask = messageService.ReceiveMessageAsync();

await Task.WhenAll(sendTask, receiveTask);
Console.WriteLine("DONE");