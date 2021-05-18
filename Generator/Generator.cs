using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Generator
{
    public class Graph
    {
        private static int InitverticesNumber = 3; //количество вершин графа-затравки для модели Барабаши-Альберта
        public static int MaxverticesNumber = 5000; //максмальное количество вершин
        private static Random randomSeed = new Random();

        public IList<int> Vertices { get; set; }

        public IDictionary<int, IList<int>> Edges { get; set; }

        private int SumOfDegree
        {
            get
            {
                int sum = 0;
                foreach (var edge in Edges)
                {
                    if (edge.Value != null)
                        sum += edge.Value.Count;
                }

                return sum;
            }
        }

        public Graph()
        {
            Vertices = new List<int>();
            Edges = new Dictionary<int, IList<int>>();
        }

        public bool GatherInput()
        {
            Console.WriteLine("Please input the max vertices number (default 5000): ");
            string inputNumberString = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputNumberString))
                return true;

            if (int.TryParse(inputNumberString, out int result))
            {
                if (result >= 0)
                {
                    MaxverticesNumber = result;
                    return true;
                }
            }

            Console.WriteLine("Not acceptable value.");
            return false;
        }

        public int GetDegreesOfVertex(int vertex) => Edges[vertex] == null ? 0 : Edges[vertex].Count;

        public int GetMaxDegree() => SetOfDegree().Max();

        public int GetMinDegree() => SetOfDegree().Min();

        public double GetMedian() //медиана
        {
            int[] valuesArray = SetOfDegree().ToArray();
            return GetAverageNum(valuesArray);
        }

        private static double GetAverageNum(int[] valuesArray)
        {
            Array.Sort(valuesArray);

            if (valuesArray.Length % 2 == 1)
            {
                return valuesArray[valuesArray.Length / 2];
            }

            return 0.5 * (valuesArray[valuesArray.Length / 2 - 1] + valuesArray[valuesArray.Length / 2]);
        }

        public double GetAveragePathLength() //средняя длина пути
        {
            long[,] a = Floyd();
            int[] b = new int[a.Length];
            int k = 0;
            for (int i = 0; i < Vertices.Count; i++)
            {
                for (int j = 0; j < Vertices.Count; j++)
                {
                    if (a[i, j] != int.MaxValue)
                    {
                        b[k] = (int)a[i, j];
                    }
                    else
                    {
                        b[k] = 0;
                    }

                    k++;
                }
            }

            return GetAverageNum(b);
        }

        public long[,] Floyd()
        {
            int i, j, k;
            //создаем массив а
            int size = Vertices.Count;
            long[,] a = GetAdjacencyMatrix();

            for (k = 0; k < size; k++)
            {
                for (i = 0; i < size; i++)
                {
                    for (j = 0; j < size; j++)
                    {
                        long distance = a[i, k] + a[k, j];
                        if (a[i, j] > distance)
                        {
                            a[i, j] = distance;
                        }
                    }
                }
            }

            return a;//в качестве результата возвращаем массив кратчайших путей между
        }

        public int Diameter() //диаметр
        {
            int n = Edges.Values.Count;
            int[,] edge = new int[n, n];
            int v = 0;
            foreach (var item in Edges.Values)
            {
                for (int i = 0; i < n; i++)
                {
                    if (i >= item.Count)
                    {
                        edge[v, i] = 10000;
                    }
                    else
                    {
                        edge[v, i] = item[i];
                    }
                }

                v++;
            }

            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (i != j)
                            edge[i, j] = Math.Min(edge[i, j], edge[i, k] + edge[k, j]);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (edge[i, j] == 10000)
                        edge[i, j] = 0;
                }
            }

            int max = edge[0, 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (edge[i, j] > max)
                        max = edge[i, j];
                }
            }

            return max;
        }

        private HashSet<int> SetOfDegree() //массив степеней
        {
            HashSet<int> degrees = new HashSet<int>();
            foreach (var item in Edges)
            {
                degrees.Add(item.Value.Count);
            }

            return degrees;
        }

        private int FindTheNextToConnect()//поиск вершины для связности
        {
            int next = randomSeed.Next(0, SumOfDegree - 1);
            int lCount = 0;

            while (lCount < Edges.Count && next >= 0)
            {
                next -= GetDegreesOfVertex(lCount++);
            }

            return lCount >= 1 ? lCount - 1 : 0;
        }

        private long[,] GetAdjacencyMatrix()
        {
            int N = Vertices.Count;
            long[,] w = new long[N, N];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Edges[Vertices[i]].Contains(Vertices[j]))
                    {
                        w[i, j] = 1;
                    }
                    else if (Vertices[i] != Vertices[j])
                    {
                        w[i, j] = Int32.MaxValue;
                    }
                    else
                    {
                        w[i, j] = 0;
                    }
                }
            }

            return w;
        }

        public void Erdos_Renyi(double p)//модель Эрдеша-Реньи по вероятности р
        {
            for (int i = 0; i < MaxverticesNumber; i++)
            {
                Vertices.Add(i);
                Edges.Add(i, new List<int>());
            }

            double r;
            for (int i = 0; i < Vertices.Count; i++)
            {
                for (int j = 0; j < Vertices.Count; j++)
                {
                    if (i < j)
                    {
                        r = randomSeed.NextDouble();
                        if (r < p)
                        {
                            Edges[i].Add(j);
                        }
                    }
                }
            }
        }

        public void Barabasi_Albert()//модель Барабаши-Альберта
        {
            GenerateInitGraphics(InitverticesNumber);

            for (int iCount = InitverticesNumber; iCount < MaxverticesNumber; iCount++)
            {
                Vertices.Add(iCount);

                int targetVertice = FindTheNextToConnect();

                Edges.Add(iCount, new List<int>() { targetVertice });
                if (Edges[targetVertice] == null)
                {
                    Edges[targetVertice] = new List<int>();
                }

                Edges[targetVertice].Add(iCount);
            }
        }

        public void Bollobas_Riordan()//модель Боллобаши-Риордана
        {
            for (int i = 0; i < MaxverticesNumber; i++)
            {
                Vertices.Add(i);
                Edges.Add(i, new List<int>());
            }

            IDictionary<int, HashSet<int>> additionalEdges = new Dictionary<int, HashSet<int>>();
            int k = randomSeed.Next(2, 5);

            GenerateGraphics(additionalEdges, MaxverticesNumber * k);//создаем граф с kn вершинами и kn 

            for (int i = 0; i < MaxverticesNumber * k; i++)
            {
                for (int j = 0; j < MaxverticesNumber * k; j++)
                {
                    int iCount = i / k, jCount = j / k;
                    if (iCount != jCount && additionalEdges[i].Contains(j) && !Edges[iCount].Contains(jCount))
                    {
                        Edges[iCount].Add(jCount);
                        Edges[jCount].Add(iCount);
                    }
                }
            }
        }

        private void GenerateGraphics(IDictionary<int, HashSet<int>> additionalEdges, int maxVerticesNumber)
        {
            int rnd;
            additionalEdges.Add(0, new HashSet<int> { 0 });
            for (int iCount = 1; iCount < maxVerticesNumber; iCount++)
            {
                additionalEdges.Add(iCount, new HashSet<int>());

                rnd = randomSeed.Next(0, iCount + 1);
                additionalEdges[iCount].Add(rnd);
                if (rnd != iCount)
                {
                    additionalEdges[rnd].Add(iCount);
                }
            }
        }

        private void GenerateInitGraphics(int initVerticesNumber)
        {
            for (int i = 0; i < initVerticesNumber; i++)
            {
                Vertices.Add(i);
                IList<int> fullEdges = new List<int>();
                for (int j = 0; j < initVerticesNumber; j++)
                {
                    if (i != j) fullEdges.Add(j);
                }

                Edges.Add(i, fullEdges);
            }
        }
    }
}
