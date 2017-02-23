using System;
using System.Fabric;
using System.Linq;

namespace SF.IntegrationTests.Coordinator
{
    class Program
    {
        static void Main(string[] args)
        {
            var fabricClient = new FabricClient();

            var applicationList = fabricClient.QueryManager.GetApplicationListAsync().GetAwaiter().GetResult();
            foreach (var application in applicationList)
            {
                var serviceList = fabricClient.QueryManager.GetServiceListAsync(application.ApplicationName).GetAwaiter().GetResult();
                foreach (var service in serviceList)
                {
                    var partitionListAsync = fabricClient.QueryManager.GetPartitionListAsync(service.ServiceName).GetAwaiter().GetResult();
                    foreach (var partition in partitionListAsync)
                    {
                        var replicas = fabricClient.QueryManager.GetReplicaListAsync(partition.PartitionInformation.Id).GetAwaiter().GetResult();
                        foreach (var replica in replicas)
                        {
                            if (!string.IsNullOrWhiteSpace(replica.ReplicaAddress))
                            {
                                Console.WriteLine($"{service.ServiceName} {replica.ReplicaAddress}");
                            }
                        }
                    }
                }
            }
        }
    }
}
