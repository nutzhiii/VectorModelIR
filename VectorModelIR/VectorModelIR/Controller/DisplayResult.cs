using System;
using System.Collections.Generic;
using System.Data;
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
            Console.WriteLine(string.Format("\nInformation: We use these {0} articles from Medium.com for demonstrating the Vector Model", documentMatrixModel.DocumentList.Count));
            for (int i = 0; i < documentMatrixModel.DocumentList.Count; i++)
            {
                Console.WriteLine(string.Format("({0}) {1}", i + 1, documentMatrixModel.DocumentList[i].Name));
            }
            Console.WriteLine("The result will show the retrieved document and similarity of Vector Model");
            Console.WriteLine("\n------------------------------\n");
        }

        public void DisplayBodyResult(TermDocumentMatrixModel documentMatrixModel, Dictionary<string, double> cosineSimilarity, string query)
        {
            
            Console.WriteLine("\n\n=[ Similarity ]=\n");
            Console.WriteLine("Query is :{0}", query);
            ConsoleTableBuilder.From(GetSampleTableData(cosineSimilarity)).ExportAndWriteLine();
        }

        public DataTable GetSampleTableData(Dictionary<string, double> cosineSimilarity)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Document Name", typeof(string));
            table.Columns.Add("Rank", typeof(double));

            var result = cosineSimilarity.OrderByDescending(e => e.Value).ToList();
            foreach (var item in result)
            {
                table.Rows.Add(item.Key, item.Value);
            }
            return table;
        }

    }
}
