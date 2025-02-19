using System;
using System.Diagnostics;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Model;
using PureCloudPlatform.Client.V2.Extensions;
using RestSharp;
using Microsoft.VisualBasic;

namespace GenesysSdkPoc 
{
    public class AnalyticsConversationsAggregates
    {

        private AnalyticsApi analyticsApi;
        
        public AnalyticsConversationsAggregates(AnalyticsApi analyticsApi)
        {
            this.analyticsApi = analyticsApi;
        }

        private ConversationAggregationQuery CreateAnalyticsConversationsAggregateQuery() 
        {
             // Create the filter predicate for mediaType = "voice"
        var predicate = new ConversationAggregateQueryPredicate
        {
            Dimension = ConversationAggregateQueryPredicate.DimensionEnum.Mediatype,
            Value = "voice"
        };

        // Create the filter object with the predicate
        var filter = new ConversationAggregateQueryFilter
        {
            Type = ConversationAggregateQueryFilter.TypeEnum.And,
            Predicates = new List<ConversationAggregateQueryPredicate> { predicate }
        };

        // Define the ConversationAggregationQuery
        var query = new ConversationAggregationQuery
        {
            Interval = "2024-11-18T13:00:00.000Z/2025-01-25T13:00:00.000Z", // ISO-8601 format
            Granularity = "PT12H", // 12-hour granularity
            GroupBy = new List<ConversationAggregationQuery.GroupByEnum>
            {
                ConversationAggregationQuery.GroupByEnum.Queueid,
                ConversationAggregationQuery.GroupByEnum.Userid
            },
            Metrics = new List<ConversationAggregationQuery.MetricsEnum>
            {
                ConversationAggregationQuery.MetricsEnum.Nconnected,
                ConversationAggregationQuery.MetricsEnum.Ntransferred,
                ConversationAggregationQuery.MetricsEnum.Tabandon
            },
            Filter = filter 
        };
            
            return query;
        }

        public void PostAnalyticsConversationsAggregates()
        {
            var body = CreateAnalyticsConversationsAggregateQuery();
            try
            { 
                // Query for conversation aggregates
                ConversationAggregateQueryResponse result = analyticsApi.PostAnalyticsConversationsAggregatesQuery(body);
                Debug.WriteLine(result.ToJson());
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling ConversationsApi.PostAnalyticsConversationsAggregatesQuery: " + e.Message );
            }
        } 
    }
}