using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace StorageEmulatorDateTimeMinValue
{
    public class Tree : TableEntity
    {
        public string Name { get; set; }
        public DateTime Planted { get; set; }
    }
}
