using System;
using System.Diagnostics;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Model;

namespace GenesysSdkPoc
{
    public class NotficationChannel
    {
        private NotificationsApi notificationsApi;

        private string uri;

        public NotficationChannel(NotificationsApi notificationsApi) 
        {
            this.notificationsApi = notificationsApi;
        }

        public String CreateNotficationChannel()
        {
            try 
            {
                Channel result = notificationsApi.PostNotificationsChannels();
                uri = result.ConnectUri;
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling NotificationsApi.PostNotificationsChannels: " + e.Message );
            }
            
            return uri;
        }

        
    }
}