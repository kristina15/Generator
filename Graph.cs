using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Barabasi_Albert_Model_CSharp_Generator
{
    public class Graph
    {
        private static int InitverticesNumber = 3; //количество вершин графа-затравки для модели Барабаши-Альберта
        private static int MaxverticesNumber = 5000; //максмальное количество вершин
        private static IList<int> vertices = new List<int>(); //список содержащий номера вершин
        private static IDictionary<int, IList<int>> edges = new Dictionary<int, IList<int>>();//словарь содержащий по ключу номер вершины, а в качестве значения список связных вершин с данной
        private static Random randomSeed = new Random();

        public IList<int> Vertices //свойство для доступа к списку вершин
        {
            get
            {
                return vertices;
            }
            set
            {
                vertices = value;
            }
        }

        public IDictionary<int, IList<int>> Edges //свойство для доступа к ребрам
        {
            get
            {
                return edges;
            }
            set
            {
                edges = value;
            }
        }

        private int GetSumDegrees(IDictionary<int, IList<int>> edges)//метод для получения суммарного количества ребер
        {
            int sum = 0;
            foreach (var edge in edges)
            {
                if (edge.Value != null)
                    sum += edge.Value.Count;
            }

            return sum;
        }

        private int GetDegreesOfVertex(IDictionary<int, IList<int>> edges, int vertex)
        {
            return edges[vertex] == null ? 0 : edges[vertex].Count;
        }

        private int FindTheNextToConnect(IDictionary<int, IList<int>> edges)//поиск вершины для связности
        {
            int next = randomSeed.Next(0, GetSumDegrees(edges) - 1);
            int lCount = 0;

            for (; lCount < edges.Count && next >= 0;)
            {
                next -= GetDegreesOfVertex(edges, lCount++);
            }

            return lCount >= 1 ? lCount - 1 : 0;
        }

        public Image generateImage()// создание изображения
        {
            Image img = new Bitmap(500, 500, PixelFormat.Format24bppRgb);
            Bitmap bmp = (Bitmap)img;
            Graphics gp = Graphics.FromImage(img);

            SolidBrush brush = new SolidBrush(Color.White);
            gp.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);

            for (int i = 0; i < vertices.Count(); i++)
            {
                float x = generateX(i, bmp.Width);
                float y = generateY(i, bmp.Height);

                gp.DrawEllipse(new Pen(Color.Black, 1), x - 5, y - 5, 10, 10);
            }

            for (int i = 0; i < edges.Count(); i++)
            {
                foreach (var item in edges[i])
                {
                    float x1 = generateX(i, bmp.Width);
                    float y1 = generateY(i, bmp.Height);
                    float x2 = generateX(item, bmp.Width);
                    float y2 = generateY(item, bmp.Height);
                    gp.DrawLine(new Pen(Color.Black, 1), x1, y1, x2, y2);
                }
            }
            return img;
        }

        private static float generateX(int id, int width)
        {
            return (float)(width / 2 + (width / 2 - 30) * Math.Cos(((Math.PI * 2) / vertices.Count()) * id));
        }

        private static float generateY(int id, int height)
        {
            return (float)(height / 2 + (height / 2 - 30) * Math.Sin(((Math.PI * 2) / vertices.Count()) * id));
        }

        public void Erdos_Renyi(double p)//модель Эрдеша-Реньи по вероятности р
        {
            for (int i = 0; i < MaxverticesNumber; i++)
            {
                Vertices.Add(i);
                Edges.Add(i, new List<int>());
            }

            double rnd;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                for (int j = i + 1; j < Vertices.Count; j++)
                {
                    rnd = randomSeed.NextDouble();
                    if (rnd < p - double.Epsilon)
                    {
                        Edges[i].Add(j);
                    }
                }
            }
        }

        public void Barabasi_Albert()//модель Барабаши-Альберта
        {
            GenerateInitGraphics(Vertices, Edges, InitverticesNumber);

            for (int iCount = InitverticesNumber; iCount < MaxverticesNumber; iCount++)
            {
                Vertices.Add(iCount);

                int targetVertice = FindTheNextToConnect(Edges);

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
            GenerateGraphics(Vertices, Edges, MaxverticesNumber);
            IList<int> vertices = new List<int>();
            IDictionary<int, IList<int>> edges = new Dictionary<int, IList<int>>();
            GenerateInitGraphics(vertices, edges, MaxverticesNumber);//инициализируем граф, состоящий из n вершин и n ребер
            int k = randomSeed.Next(2, 5);
            
            GenerateGraphics(vertices, edges, MaxverticesNumber * k);//создаем граф с kn вершинами и kn 
            for (int iCount = MaxverticesNumber; iCount < MaxverticesNumber * k; iCount++)
            {
                int targetValue = 0;
                do
                {
                    targetValue = randomSeed.Next(0, MaxverticesNumber * k);
                }
                while (targetValue == iCount);
                edges[iCount].Add(targetValue);
                edges[targetValue].Add(iCount);
            }

            for (int i = 0; i < MaxverticesNumber * k; i++)
            {
                for (int j = 0; j < MaxverticesNumber * k; j++)
                {
                    if (i / k != j / k && edges[i].Contains(j))
                    {
                        Edges[i / k].Add(j / k);
                    }
                }
            }
        }

        private void GenerateGraphics(IList<int> vertices, IDictionary<int, IList<int>> edges, int maxVerticesNumber)
        {
            int m = vertices.Count;
            for (int iCount = m; iCount < maxVerticesNumber; iCount++)
            {
                vertices.Add(iCount);
                edges.Add(iCount, new List<int>());
            }
        }

        public void GenerateInitGraphics(IList<int> vertices, IDictionary<int, IList<int>> edges, int initVerticesNumber)
        {
            for (int iCount = 0; iCount < initVerticesNumber; iCount++)
            {
                vertices.Add(iCount);
            }

            for (int lCount = 0; lCount < initVerticesNumber; lCount++)
            {
                IList<int> fullEdges = new List<int>();
                for (int iCount = 0; iCount < initVerticesNumber; iCount++)
                {
                    if (iCount != lCount)
                        fullEdges.Add(iCount);
                }

                edges.Add(lCount, fullEdges);
            }
        }

        public Boolean GatherInput()
        {
            Console.WriteLine("Please input the max vertices number (default 5000): ");
            String inputNumberString = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(inputNumberString))
                return true;

            int result = -1;
            if (int.TryParse(inputNumberString, out result))
                if (result >= 0)
                {
                    MaxverticesNumber = result;
                    return true;
                }

            Console.WriteLine("Not acceptable value.");
            return false;
        }
    }
}