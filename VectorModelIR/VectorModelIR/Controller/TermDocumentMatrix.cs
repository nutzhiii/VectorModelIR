using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorModelIR
{
    public class TermDocumentMatrix
    {
        public TermDocumentMatrixModel SettingTermDocumentMatrix(TermDocumentMatrixModel documentMatrix)
        {
            Dictionary<string, int> IsTermInDocument = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, double>> TFIndexTermOfDocumentList = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, double> IDFIndexTermOfDocument = new Dictionary<string, double>();
            Dictionary<string, Dictionary<string, double>> TF_IDFIndexTermOfDocumentList = new Dictionary<string, Dictionary<string, double>>();
            //Term all
            foreach (var item in documentMatrix.IndexTermList)
            {
                int countTermInDocument = 0;
                Dictionary<string, double> TFIndexTermOfDocument = new Dictionary<string, double>();
                Dictionary<string, double> TF_IDFIndexTermOfDocument = new Dictionary<string, double>();
                //document
                foreach (var indexdocterm in documentMatrix.DocumentList)
                {
                    //Ni
                    if (indexdocterm.IndexTermDocuments.ContainsKey(item))
                    {
                        countTermInDocument++;
                        //Calculate TF
                        TFIndexTermOfDocument.Add(indexdocterm.Name, CalculateTF(indexdocterm.IndexTermDocuments[item]));
                    }
                    else
                    {
                        //TF = 0 
                        TFIndexTermOfDocument.Add(indexdocterm.Name, 0);
                    }
                }
                //Calculate IDF
                IDFIndexTermOfDocument.Add(item, CalculateIDF(countTermInDocument, documentMatrix.DocumentList.Count()));
                IsTermInDocument.Add(item, countTermInDocument);
                TFIndexTermOfDocumentList.Add(item, TFIndexTermOfDocument);


                foreach (var document in documentMatrix.DocumentList)
                {
                    //calulate IF_IDF
                    var tfOfDocumentList = TFIndexTermOfDocumentList[item];
                    var result = CalculateTF_IDF(tfOfDocumentList[document.Name], IDFIndexTermOfDocument[item]);
                    TF_IDFIndexTermOfDocument.Add(document.Name, result);
                    document.VectorNorm = document.VectorNorm + Math.Pow(result, 2);
                }
                TF_IDFIndexTermOfDocumentList.Add(item, TF_IDFIndexTermOfDocument);
            }
            //VectorNorm
            foreach (var document in documentMatrix.DocumentList)
            {
                document.VectorNorm = Math.Round(Math.Sqrt(document.VectorNorm), 3);
            }
            documentMatrix.IDFIndexTermOfDocument = IDFIndexTermOfDocument;
            documentMatrix.TFIndexTermOfDocument = TFIndexTermOfDocumentList;
            documentMatrix.IsTermInDocument = IsTermInDocument;
            documentMatrix.TF_IDFIndexTermOfDocument = TF_IDFIndexTermOfDocumentList;
            return documentMatrix;
        }

        public double CalculateTF(int frequency)
        {
            double result = 0.0;
            if (frequency > 0)
            {
                result = Math.Round(1 + Math.Log(frequency, 2.0), 3);
            }
            return result;
        }

        public double CalculateIDF(double numberOfTermIndocument, double numberOfDocument)
        {
            double result = 0.0;
            double sumOfNumberOfTermDividNumberOfDocument = 0.0;
            if (numberOfTermIndocument > 0)
            {
                sumOfNumberOfTermDividNumberOfDocument = numberOfDocument / numberOfTermIndocument;
                result = Math.Round(Math.Log(sumOfNumberOfTermDividNumberOfDocument, 2.0), 3);
            }
            return result;
        }

        public double CalculateTF_IDF(double tfi, double idf)
        {
            double result = 0.0;
            double crossTfiAndIdf = 0.0;
            crossTfiAndIdf = tfi * idf;
            result = Math.Round(crossTfiAndIdf, 3);
            return result;
        }
    }
}
