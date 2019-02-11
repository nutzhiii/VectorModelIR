using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VectorModelIR
{
    public class CalculateSimilarityCosine
    {
        public Dictionary<string, double> CalculateSimilarity(TermDocumentMatrixModel documentMatrix, string query)
        {
            Dictionary<string, int> isTermInQuery = CountFrequencyOfTermQuery(query);
            Dictionary<string, double> tfOfQuery = new Dictionary<string, double>();
            Dictionary<string, double> idfOfQuery = new Dictionary<string, double>();
            Dictionary<string, double> tf_idfOfQuery = new Dictionary<string, double>();
            double vectorNorm = 0.0;
            //tf of query , idf of query , tf_idf of query , vectorNorm
            foreach (var item in isTermInQuery)
            {
                int numberOfTermInDocument = 0;
                tfOfQuery.Add(item.Key, new TermDocumentMatrix().CalculateTF(item.Value));
                if (documentMatrix.IsTermInDocument.ContainsKey(item.Key))
                {
                    numberOfTermInDocument = documentMatrix.IsTermInDocument[item.Key];
                }
                idfOfQuery.Add(item.Key, new TermDocumentMatrix().CalculateIDF(numberOfTermInDocument, documentMatrix.DocumentList.Count()));
                tf_idfOfQuery.Add(item.Key, new TermDocumentMatrix().CalculateTF_IDF(tfOfQuery[item.Key], idfOfQuery[item.Key]));
                vectorNorm = vectorNorm + Math.Pow(tf_idfOfQuery[item.Key], 2);
            }
            vectorNorm = Math.Round(Math.Sqrt(vectorNorm), 3);
            //cosine similarity
            var result = CalcaulateCosineSimilarity(tf_idfOfQuery, vectorNorm, documentMatrix);
            return result;
        }

        public Dictionary<string, int> CountFrequencyOfTermQuery(string query)
        {
            Dictionary<string, int> freqencyOfTermQuery = new Dictionary<string, int>();
            List<string> splitQuery = Regex.Replace(query, @"[^\w\s]", string.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var item in splitQuery)
            {
                if (freqencyOfTermQuery.ContainsKey(item))
                {
                    freqencyOfTermQuery[item] = freqencyOfTermQuery[item] + 1;
                }
                else
                {
                    freqencyOfTermQuery.Add(item, 1);
                }
            }
            return freqencyOfTermQuery;
        }

        public Dictionary<string, double> CalcaulateCosineSimilarity(Dictionary<string, double> tf_idfQuery, double vectorNorm, TermDocumentMatrixModel documentMatrix)
        {
            Dictionary<string, double> cosineSimilarity = new Dictionary<string, double>();
            foreach (var item in documentMatrix.DocumentList)
            {
                //follow by document
                double crossWeight = 0.0;
                double crossSumVectorNorm = 0.0;
                double cosineSimilarityOfDocument = 0.0;
                foreach (var tf_idfQItem in tf_idfQuery)
                {
                    if (documentMatrix.TF_IDFIndexTermOfDocument.ContainsKey(tf_idfQItem.Key))
                    {
                        var tF_IDFList = documentMatrix.TF_IDFIndexTermOfDocument[tf_idfQItem.Key];
                        crossWeight = crossWeight + (tF_IDFList[item.Name] * tf_idfQItem.Value);
                    }
                    else
                    {
                        crossWeight = crossWeight + (0 * tf_idfQItem.Value);
                    }
                }
                crossSumVectorNorm = Math.Round(item.VectorNorm * vectorNorm, 3);
                cosineSimilarityOfDocument = Math.Round(crossWeight / crossSumVectorNorm, 3);
                cosineSimilarity.Add(item.Name, cosineSimilarityOfDocument);
            }
            return cosineSimilarity;
        }
    }
}
