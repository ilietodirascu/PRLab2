using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Aggregator
{
    public class Simulation
    {
        public static ConcurrentQueue<Product> ProducerProducts { get; set; } = new();
        private static readonly Object _lock = new();
        public static Random Random { get; set; } = new();
        public static HttpClient Client { get; set; } = new();
        public static ConcurrentQueue<Product> ConsumerProducts { get; set; } = new();
        public static List<Thread> Threads { get; set; }

        static Simulation()
        {
            Threads = new();
            for (int i = 0; i < 7; i++)
            {
                Threads.Add(new Thread(ThreadTask));
                Threads[i].Name = "Aggregator Thread " + i;
            }
            Threads.ForEach(x => x.Start());
        }
        public static void ThreadTask()
        {

            while (true)
            {
                Thread.Sleep(Random.Next(1, 11) * 1000);
                lock (_lock)
                {
                    if (!ProducerProducts.IsEmpty)
                    {
                        ProducerProducts.TryDequeue(out Product product);
                        var newProduct = new Product("Aggregator to Consumer", product.ProductNumber, Thread.CurrentThread.Name );
                        Client.PostAsJsonAsync("http://localhost:5001/LogInfo", newProduct);
                        Client.PostAsJsonAsync("http://localhost:5002/AddAggregatorData", newProduct);
                    }
                    if (!ConsumerProducts.IsEmpty)
                    {
                        ConsumerProducts.TryDequeue(out Product product);
                        var newProduct = new Product("Aggregator to Producer", product.ProductNumber, Thread.CurrentThread.Name);
                        Client.PostAsJsonAsync("http://localhost:5001/LogInfo", newProduct);
                        Client.PostAsJsonAsync("http://localhost:5000/AddAggregatorData", newProduct);
                    }
                }
            }
        }
    }
}
