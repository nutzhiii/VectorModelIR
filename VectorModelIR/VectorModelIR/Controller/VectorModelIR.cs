using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorModelIR
{
    public class VectorModelIR
    {
        static void Main(string[] args)
        {
            //document detail contain readfile and filter index term document 
            TermDocumentMatrixModel documentMatrix = new Files().ReadFile();
            //Display head
            new DisplayResult().DisplayHead(documentMatrix);
            //setting term document matrix
            documentMatrix = new TermDocumentMatrix().SettingTermDocumentMatrix(documentMatrix);

            //Query
            while (true)
            {
                Console.Write("Enter your query: ");
                string query = Console.ReadLine().ToLower();
                Dictionary<string, double> cosineSimilarity = new CalculateSimilarityCosine().CalculateSimilarity(documentMatrix, query);
                //Display body result
                new DisplayResult().DisplayBodyResult(documentMatrix, cosineSimilarity, query);
            }
        }


    }
}
