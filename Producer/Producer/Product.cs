using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer
{
    public class Product
    {
        public string Data { get; set; }
        public string ServerName { get; set; }
        public int ProductNumber { get; set; }
        public string ThreadName { get; set; }

        public Product(string serverName, int productNumber, string threadName)
        {
            ServerName = serverName;
            ProductNumber = productNumber;
            ThreadName = threadName;
            Data = $"{threadName} \n" +
                        $"{serverName} sends Package Nr: {productNumber}";
        }
        public string GetData()
        {
            return $"{ThreadName} \n" +
                        $"{ServerName} sends Package Nr: {ProductNumber}";
        }
    }
}
