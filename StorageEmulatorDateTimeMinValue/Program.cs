using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace StorageEmulatorDateTimeMinValue
{
    class Program
    {
        static async Task Main()
        {
            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = storageAccount.CreateCloudTableClient();

            var table = client.GetTableReference("Trees");
            await table.DeleteIfExistsAsync();
            await table.CreateIfNotExistsAsync();
            
            var trees = new Tree[]
            {
                new Tree { PartitionKey = "Forest", RowKey = "Beech", Planted = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Tree { PartitionKey = "Forest", RowKey = "Fir", Planted = new DateTime(1701, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Tree { PartitionKey = "Forest", RowKey = "Spruce", Planted = new DateTime(1751, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Tree { PartitionKey = "Forest", RowKey = "Oak", Planted = new DateTime(1752, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Tree { PartitionKey = "Forest", RowKey = "Larch", Planted = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new Tree { PartitionKey = "Forest", RowKey = "Pine", Planted = new DateTime(1754, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            };
            var batchOperation = new TableBatchOperation();
            foreach (var tree in trees)
            {
                batchOperation.Insert(tree);
            }
            await table.ExecuteBatchAsync(batchOperation);

            foreach (var tree in trees)
            {
                var retrieveOperation = TableOperation.Retrieve<Tree>(tree.PartitionKey, tree.RowKey);
                var storageTreeTableResult = await table.ExecuteAsync(retrieveOperation);
                var storageTree = storageTreeTableResult.Result as Tree;
                Console.WriteLine($"Expected year {tree.Planted.Year} and got {storageTree.Planted.Year}.");
            }
        }
    }
}
