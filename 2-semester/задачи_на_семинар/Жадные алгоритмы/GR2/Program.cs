public class CityNetwork
{
    private class Edge
    {
        public int Target { get; set; }
        public double Time { get; set; }

        public Edge(int target, double time)
        {
            Target = target;
            Time = time;
        }
    }

    private class PriorityQueue<T>
    {
        private readonly List<(T Item, double Priority)> elements = new();

        public int Count => elements.Count;

        public void Enqueue(T item, double priority)
        {
            elements.Add((item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 1; i < elements.Count; i++)
            {
                if (elements[i].Priority < elements[bestIndex].Priority)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }

    private readonly List<Edge>[] graph;

    public CityNetwork(int numberOfCities)
    {
        graph = new List<Edge>[numberOfCities];
        for (int i = 0; i < numberOfCities; i++)
        {
            graph[i] = new List<Edge>();
        }
    }

    public void AddRoad(int city1, int city2, double length, double speed)
    {
        double time = length / speed;
        graph[city1].Add(new Edge(city2, time));
        graph[city2].Add(new Edge(city1, time));
    }

    public double CalculateMinimumTime(int capitalCity)
    {
        double[] minTime = new double[graph.Length];
        for (int i = 0; i < minTime.Length; i++)
        {
            minTime[i] = double.MaxValue;
        }

        minTime[capitalCity] = 0;

        PriorityQueue<int> priorityQueue = new();
        priorityQueue.Enqueue(capitalCity, 0);

        while (priorityQueue.Count > 0)
        {
            int currentCity = priorityQueue.Dequeue();

            foreach (var edge in graph[currentCity])
            {
                double newTime = minTime[currentCity] + edge.Time;
                if (newTime < minTime[edge.Target])
                {
                    minTime[edge.Target] = newTime;
                    priorityQueue.Enqueue(edge.Target, newTime);
                }
            }
        }

        return minTime.Max();
    }
}

public class Program
{
    public static void Main()
    {
        int numberOfCities = 5;
        CityNetwork cityNetwork = new(numberOfCities);

        cityNetwork.AddRoad(0, 1, 100, 50);
        cityNetwork.AddRoad(0, 2, 200, 50);
        cityNetwork.AddRoad(1, 3, 150, 75);
        cityNetwork.AddRoad(2, 4, 300, 60);
        cityNetwork.AddRoad(3, 4, 100, 100);

        int capitalCity = 0;
        double minimumTime = cityNetwork.CalculateMinimumTime(capitalCity);
        Console.WriteLine($"Минимальное время для распространения информации: {minimumTime}");
    }
}
