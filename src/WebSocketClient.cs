using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using PureCloudPlatform.Client.V2.Api;

namespace GenesysSdkPoc
{
    public class WebSocketClient
    {
         private ClientWebSocket ws = new ClientWebSocket();

         private readonly string uriString;

         public WebSocketClient(string uriString) 
         {
            this.uriString = uriString;
         }

         async public Task ConnectToWebsocket(Action action) 
         {

            try 
            {
                await ws.ConnectAsync(new Uri(uriString), CancellationToken.None);
                Debug.WriteLine("Connected");

                var receiveTask = Task.Run(async () =>
                {
    
                    // send manual health check
                    var  message = "{\"message\": \"ping\"}";
                    var encodeByte = Encoding.UTF8.GetBytes(message);

                    Debug.WriteLine("Sending health check message");
                    await ws.SendAsync(new ArraySegment<byte>(encodeByte), WebSocketMessageType.Text, true, CancellationToken.None); 
                    Debug.WriteLine("Health check message sent");

                    // recieve health check
                    var buffer = new byte[1024];
                    while (ws.State == WebSocketState.Open) 
                    {
                        try 
                        {
                            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                action.Invoke();
                            }
                            else if (result.MessageType == WebSocketMessageType.Close)
                            {
                            Console.WriteLine("WebSocket connection closed by server.");
                            }
                        }
                        catch (Exception e) 
                        {
                            Console.WriteLine($"Error: {e.Message}");
                        }

                    }
                }); 
                
                await receiveTask;
    
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }            
         }
    }
}