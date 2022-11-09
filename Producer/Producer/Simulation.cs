using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    public class Simulation
    {
        private static readonly Object _senderLock = new();
        public static List<Thread> SenderThreads { get; set; }
        public static List<Thread> ReceiverThreads { get; set; }
        public static int ProductNumber { get; set; } = 1;
        public static HttpClient Client { get; set; } = new();
        public static Random Random { get; set; } = new();
        public static ConcurrentQueue<Product> AggregatorProducts { get; set; } = new();

        static Simulation()
        {
            SenderThreads = new();
            ReceiverThreads = new();
            for (int i = 0; i < 7; i++)
            {
                SenderThreads.Add(new Thread(SenderTask));
                SenderThreads[i].Name = "Producer Thread " + i;
                ReceiverThreads.Add(new Thread(ReceiverTask));
                ReceiverThreads[i].Name = "Receiver Thread " + i;
            }
            SenderThreads.ForEach(x => x.Start());
            ReceiverThreads.ForEach(x => x.Start());
        }
        public static void SenderTask()
        {

            while (true)
            {
                Thread.Sleep(Random.Next(1, 11) * 1000);
                lock (_senderLock)
                {
                    if (ProductNumber > 10) break;
                    var product = new Product("Producer Server To Aggregator", ProductNumber, Thread.CurrentThread.Name);
                    Client.PostAsJsonAsync("http://localhost:5001/AddProducerData", product);
                    Client.PostAsJsonAsync("http://localhost:5000/LogInfo", product);
                    ProductNumber++;
                }
            }
        }
        public static void ReceiverTask()
        {
            while (true)
            {
                Thread.Sleep(Random.Next(1, 11) * 1000);
                if (AggregatorProducts.IsEmpty) continue;
                AggregatorProducts.TryDequeue(out Product product);
                var finalProduct = new Product("Final Producer", product.ProductNumber, Thread.CurrentThread.Name);
                Client.PostAsJsonAsync("http://localhost:5000/LogInfo", finalProduct);
            }
        }
    }
}
