using System.Diagnostics;
using System.Net.WebSockets;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Extensions;
using GenesysSdkPoc;
using System.Threading.Channels;
using PureCloudPlatform.Client.V2.Model;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Authenticaiton using Client Credential Oauth token created on Genesys Cloud 
        var accessTokenInfo = Configuration.Default.ApiClient.PostToken("", "");

        // Setting purecloud org region 
        PureCloudRegionHosts region = PureCloudRegionHosts.us_east_1;
        Configuration.Default.ApiClient.setBasePath(region);

        /** MARK 1: Querying Conversation API to get metric details [Nconneted, Ntransffered, Tabandon] on interation  filtering to mediatype and user id **/

        var analyticsApi = new AnalyticsApi();
        var conversationAnalyticsAggregate = new AnalyticsConversationsAggregates(analyticsApi);
        conversationAnalyticsAggregate.PostAnalyticsConversationsAggregates();

        /** MARK 2: Streaming ongoing interactions on a Queue via WebsocketI  **/   
        
        // Create a notification channel to subscribe to conversation notifications
        var notificationsApi = new NotificationsApi();
        var notificationChannel = new NotficationChannel(notificationsApi);
        var channelUri = notificationChannel.CreateNotficationChannel();
        Console.WriteLine("Channel URI: " + channelUri);

        // Subscribe to a converstion notication for the queue
        var queueId = "";
        var channelId = channelUri.Split("/").Last();
        var channelTopic = new ChannelTopic 
        {
            Id = $"v2.routing.queues.{queueId}.conversations"
        };
        var topics = new List<ChannelTopic>(){ channelTopic };

        try
        {
            ChannelTopicEntityListing result = notificationsApi.PutNotificationsChannelSubscriptions(channelId, topics, true);
        }
        catch (Exception e)
        {
            Debug.Print("Exception when calling NotificationsApi.PutNotificationsChannelSubscriptions: " + e.Message );
        }

    
        // connect the web socket for live data streaming
        var websocketClient = new WebSocketClient(channelUri);
        await websocketClient.ConnectToWebsocket();

        var queueObservationQuery = new AnalyticsQueryObservation(analyticsApi, queueId);
        queueObservationQuery.CreateAnalyticsQueuesObservationsQuery();
    }
}