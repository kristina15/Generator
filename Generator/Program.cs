using System;
using System.Collections.Generic;
using System.IO;

namespace Generator
{
    class Program
    {
        static void Main()
        {
            Graph graph = new Graph();
            //while (!graph.GatherInput())
            //    ;
            //graph.Barabasi_Albert();
            //double p = 1;
            Graph.MaxverticesNumber = 1876;
            //graph.Erdos_Renyi(p);
            graph.Bollobas_Riordan();
            //Image img = graph.generateImage();
            //img.Save(@"C:\Users\krisy\OneDrive\Documents\SiAKOD\GraphGenerator\Новый точечный рисунок.jpg");
            //Dictionary<int, int> Vertices = new Dictionary<int, int>();
            //Dictionary<int, List<int>> Edges = new Dictionary<int, List<int>>();
            //int i1 = 0;
            //using (StreamReader reader = new StreamReader(@"C:\Users\krisy\OneDrive\Documents\Generator\CA-AstroPh.txt"))
            //{
            //    string str1 = reader.ReadLine();
            //    while (!string.IsNullOrEmpty(str1))
            //    {
            //        string[] str = str1.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            //        int k = Convert.ToInt32(str[0]);
            //        if (!Vertices.ContainsKey(k))
            //        {
            //            Vertices.Add(k, i1);
            //            Edges.Add(i1, new List<int>());
            //            i1++;
            //        }

            //        int k2 = Convert.ToInt32(str[1]);
            //        if (!Vertices.ContainsKey(k2))
            //        {
            //            Vertices.Add(k2, i1);
            //            Edges.Add(i1, new List<int>());
            //            i1++;
            //        }

            //        int res1 = Vertices[k];
            //        int res2 = Vertices[k2];
            //        if (!Edges[res1].Contains(res2))
            //        {
            //            Edges[res1].Add(res2);
            //        }

            //        if (!Edges[res2].Contains(res1))
            //        {
            //            Edges[res2].Add(res1);
            //        }

            //        str1 = reader.ReadLine();
            //    }
            //}

            //graph.Edges = Edges;
            //foreach (var item in Vertices.Keys)
            //{
            //    graph.Vertices.Add(item);
            //}

            Console.WriteLine(graph.GetAveragePathLength());
        }
    }
}