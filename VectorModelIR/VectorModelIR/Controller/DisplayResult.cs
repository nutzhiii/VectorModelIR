using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;

namespace VectorModelIR
{
    public class DisplayResult
    {
        public void DisplayHead(TermDocumentMatrixModel documentMatrixModel)
        {
            Console.WriteLine("==[ Vector Model ]==");
            Console.WriteLine(string.Format("\nInformation: We use these {0} articles from Medium.com for demonstrating the Boolean Model", documentMatrixModel.DocumentList.Count));
            for (int i = 0; i < documentMatrixModel.DocumentList.Count; i++)
            {
                Console.WriteLine(string.Format("({0}) {1}", i + 1, documentMatrixModel.DocumentList[i].Name));
            }
            Console.WriteLine("The result will show the retrieved document and similarity of Vector Model");
            Console.WriteLine("\n------------------------------\n");
        }

        public void DisplayBodyResult(TermDocumentMatrixModel documentMatrixModel, Dictionary<string, double> cosineSimilarity, string query)
        {
            Console.WriteLine("\n\n=[ Retrieved Document ]=");
            Console.WriteLine("\n\n=[ Similarity ]=\n");
            Console.WriteLine("Query is :{0}", query);
            Console.WriteLine(string.Format("[Document Name]\t\t\t\t[Rank]"));
            var result = cosineSimilarity.OrderByDescending(e => e.Value);
            foreach (var item in result)
            {
                Console.WriteLine(string.Format("{0}\t\t{1}", item.Key, item.Value));
            }
            Console.WriteLine("\n\n------------------------------\n");
        }

    }
}
