using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Threading.Channels;
using LeetCode;

class Program
{
    private static Queue<Robot> _robots = new Queue<Robot>();
    private readonly object _lock = new object();
    
    static async Task Main()
    {
        var employeeScores = new ThreadSafeCache<string, int>();

        await Task.WhenAll(Task.Run(
            () => employeeScores.AddOrUpdate("Alice", 95)
        ));

        var searchKey = "Alice";

        employeeScores.TryGetValue("Alice", out int score);
        
        // Create an unbounded channel (acts as our thread-safe queue)
        var channel = Channel.CreateUnbounded<string>();

        /*
        // Start Producer Task
        Task producer = Task.Run(async () =>
        {
            for (int i = 1; i <= 5; i++)
            {
                string workItem = $"Job-{i}";
                await channel.Writer.WriteAsync(workItem);
                Console.WriteLine($"[Producer] Enqueued: {workItem}");
                await Task.Delay(500); // Simulate work
            }
            channel.Writer.Complete(); // Signal that no more items are coming
        });

        // Start Consumer Task
        Task consumer = Task.Run(async () =>
        {
            // ReadAllAsync automatically loops until the writer completes
            await foreach (var item in channel.Reader.ReadAllAsync())
            {
                Console.WriteLine($"[Consumer] Processing: {item} on Thread ID: {Environment.CurrentManagedThreadId}");
                await Task.Delay(800); // Simulate heavy processing
                Console.WriteLine($"[Consumer] Finished: {item}");
            }
        });

        await Task.WhenAll(producer, consumer);
        Console.WriteLine("All queue processing completed.");
        */
        
        /*
        var message = "Hello from LeetCode!";
        Console.WriteLine(message);

        var tasks = new Task[5];
        for (int i = 0; i < 5; i++)
        {
            int id = i + 1;
            tasks[i] = Task.Run(() => UpdateCounterAsync(id));
        }

        await Task.WhenAll(tasks);
        */
    }
    
    public void Enqueue(Robot item)
    {
        lock (_lock)
        {
            // Wait while queue is full
            /*
            while (_robots.Count >= _maxCapacity)
            {
                Monitor.Wait(_lock); // Release lock and block until space opens up
            }
            */
            _robots.Enqueue(item);
            //Monitor.PulseAll(_lock); // Wake up waiting consumers
        }
    }

    public Robot Dequeue()
    {
        lock (_lock)
        {
            // Wait while queue is empty
            /*
            while (_queue.Count == 0)
            {
                Monitor.Wait(_lock); // Release lock and block until items arrive
            }
            */

            Robot item = _robots.Dequeue();
            //Monitor.PulseAll(_lock); // Wake up waiting producers
            return item;
        }
    }
}

public class SimpleThreadSafeQueue<T>
{
    private readonly Queue<T> _queue = new Queue<T>();
    private readonly object _lock = new object();
    private readonly int _maxCapacity;

    public SimpleThreadSafeQueue(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
    }

    public void Enqueue(T item)
    {
        lock (_lock)
        {
            // Wait while queue is full
            while (_queue.Count >= _maxCapacity)
            {
                Monitor.Wait(_lock); // Release lock and block until space opens up
            }

            _queue.Enqueue(item);
            Monitor.PulseAll(_lock); // Wake up waiting consumers
        }
    }

    public T Dequeue()
    {
        lock (_lock)
        {
            // Wait while queue is empty
            while (_queue.Count == 0)
            {
                Monitor.Wait(_lock); // Release lock and block until items arrive
            }

            T item = _queue.Dequeue();
            Monitor.PulseAll(_lock); // Wake up waiting producers
            return item;
        }
    }
}