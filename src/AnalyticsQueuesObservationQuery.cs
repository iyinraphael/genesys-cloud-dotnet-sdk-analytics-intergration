using System;
using System.Diagnostics;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Client;
using PureCloudPlatform.Client.V2.Model;

namespace GenesysSdkPoc
{
    public class AnalyticsQueryObservation
    {
        private AnalyticsApi analyticsApi;

        private string queueId;


        private QueueObservationQuery CreateQueueObservationQuery()
        {
            var predicate = new QueueObservationQueryPredicate
            {
                Dimension = QueueObservationQueryPredicate.DimensionEnum.Queueid,
                Value = queueId
            };
            
            var clause = new QueueObservationQueryClause
            {
                Type = QueueObservationQueryClause.TypeEnum.Or,
                Predicates = [predicate]
            };

            var filter = new QueueObservationQueryFilter
            {
                Type = QueueObservationQueryFilter.TypeEnum.And,
                Clauses = [clause]
            };

            return new QueueObservationQuery
            {
                Filter = filter,
                DetailMetrics = [QueueObservationQuery.DetailMetricsEnum.Ointeracting],
                Metrics = [QueueObservationQuery.MetricsEnum.Ointeracting
                ,QueueObservationQuery.MetricsEnum.Owaiting, 
                QueueObservationQuery.MetricsEnum.Ouserroutingstatuses]
            };
        }

        public AnalyticsQueryObservation(AnalyticsApi analyticsApi, string queueId)
        {
            this.analyticsApi = analyticsApi;
            this.queueId = queueId;
        }


        public void FetchAnalyticsQueuesObservationsMetrics()
        {
            try
            { 
                var body = CreateQueueObservationQuery();
                QueueObservationQueryResponse result = analyticsApi.PostAnalyticsQueuesObservationsQuery(body);
                Debug.WriteLine(result.ToJson());
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalyticsApi.PostAnalyticsQueuesObservationsQuery: " + e.Message );
            }
        }

    }
}