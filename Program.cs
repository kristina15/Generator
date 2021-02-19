using System.Drawing;
using Barabasi_Albert_Model_CSharp_Generator;

namespace Barabasi_Albert_Model_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            while (!graph.GatherInput())
                ;
            graph.Barabasi_Albert();
            double p = 0.4;
            graph.Erdos_Renyi(p);
            graph.Bollobas_Riordan();
            Image img = graph.generateImage();
            img.Save(@"C:\Users\krisy\OneDrive\Documents\SiAKOD\GraphGenerator\Новый точечный рисунок.jpg");
        }
    }
}