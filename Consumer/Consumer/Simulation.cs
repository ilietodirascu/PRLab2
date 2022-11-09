using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    public class Simulation
    {
        public static ConcurrentQueue<Product> AggregatorProducts { get; set; } = new();
        private static readonly Object _lock = new();
        public static Random Random { get; set; } = new();
        public static HttpClient Client { get; set; } = new();
        public static List<Thread> Threads { get; set; }

        static Simulation()
        {
            Threads = new();
            for (int i = 0; i < 7; i++)
            {
                Threads.Add(new Thread(ThreadTask));
                Threads[i].Name = "Consumer Thread " + i;
            }
            Threads.ForEach(x => x.Start());
        }
        public static void ThreadTask()
        {
            lock (_lock)
            {
                while (true)
                {
                    Thread.Sleep(Random.Next(1, 11) * 1000);
                    if (!AggregatorProducts.IsEmpty)
                    {
                        if (AggregatorProducts.TryDequeue(out Product product))
                        {
                            var newProduct = new Product("Consumer to Aggregator", product.ProductNumber, Thread.CurrentThread.Name);
                            Client.PostAsJsonAsync("http://localhost:5002/LogInfo", newProduct);
                            Client.PostAsJsonAsync("http://localhost:5001/AddConsumerData", newProduct);
                        }
                    }
                }
            }
        }
    }
}
