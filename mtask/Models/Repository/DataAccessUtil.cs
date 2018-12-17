using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using mtask.Models.DataModel;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace mtask.Models.Repository
{
    public static class DataAccessUtil
    {
        private static string GetConnectionString()
        {
            return CloudConfigurationManager.GetSetting("StorageConnectionString");
        }

        private static CloudTableClient GetTableClient()
        {
            var connectionSetting = GetConnectionString();
            return CloudStorageAccount.Parse(connectionSetting).CreateCloudTableClient();
        }

        public static CloudTable GetTable(string name)
        {
            var client = GetTableClient();
            var table = client.GetTableReference(name);
            table.CreateIfNotExists();
            return table;
        }

        public static void InsertOrReplace<T>(CloudTable table, T element)
            where T : ITableEntity
        {
            var insertOperation = TableOperation.InsertOrReplace(element);
            table.Execute(insertOperation);
        }

        public static IEnumerable<T> Retrieve<T>(CloudTable table, string partitionKey = null)
            where T : ITableEntity, new()
        {
            if (partitionKey == null)
            {
                TableContinuationToken token = null;
                do
                {
                    var result = table.ExecuteQuerySegmented(new TableQuery<T>(), token);
                    foreach (var entity in result.Results)
                        yield return entity;
                    token = result.ContinuationToken;
                }
                while (token != null);
            }
            else
            {
                TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
                foreach (T entity in table.ExecuteQuery(query))
                    yield return entity;
            }
        }

        public static T Retrieve<T>(CloudTable table, string partitionKey, string rowKey)
            where T : ITableEntity
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var retrievedResult = table.Execute(retrieveOperation);
            return (T)retrievedResult.Result;
        }
    }
}