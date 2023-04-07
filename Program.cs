namespace Program
{
    class Program
    {

        private static Queue<int> stream = new Queue<int>();
        private static Random random = new Random();
        private static readonly Object o = new Object();
        private static void produce()
        {
            int input;
            lock(o)
            {
                while (true)
                {
                    if (stream.Count > 40)
                        Monitor.PulseAll(o);
                    if (stream.Count > 100)
                    {
                        Monitor.PulseAll(o);
                        Monitor.Wait(o);
                    }
                    input = random.Next(1, 100);
                    Console.WriteLine("PRODUCE");
                    stream.Enqueue(input);
                }
            }
        }

        private static void consume()
        {
            lock (o)
            {
                while (true)
                {
                    Console.WriteLine("CONSUME");
                    if (stream.Count == 0)
                    {
                        Monitor.PulseAll(o);
                        Monitor.Wait(o);
                    }
                    
                    if (stream.Count < 30)
                    Monitor.Pulse(o);
                    stream.Dequeue();
                }
                
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(produce);
            Thread t2 = new Thread(consume);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            
        }
    }
}