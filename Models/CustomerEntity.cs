using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos.Table;

namespace APIMVCDemo.Models
{
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity() { }

        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public string Occupation { get; set; }

        public string ImageURL { get; set; }

        public string Email { get; set; }
    }
}
