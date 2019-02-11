﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis;

namespace VectorModelIR
{
    public class Files
    {
        List<string> indexTermList = new List<string>();
        public TermDocumentMatrixModel ReadFile()
        {
            string localPath = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName, @"Appdata");
            List<string> filePathList = new List<string>(Directory.EnumerateFiles(localPath, "*.txt"));
            List<DocumentModel> documentList = new List<DocumentModel>();

            foreach (var filePath in filePathList)
            {
                var content = File.ReadAllText(filePath).ToLower();
                documentList.Add(new DocumentModel { Name = Path.GetFileNameWithoutExtension(filePath), Content = File.ReadAllText(filePath).ToLower(), IndexTermDocuments = FilterIndexTermDocument(content) });
            }
            indexTermList = indexTermList.Distinct().ToList();
            return new TermDocumentMatrixModel() { IndexTermList = indexTermList, DocumentList = documentList };
        }
        public Dictionary<string, int> FilterIndexTermDocument(string content)
        {
            Dictionary<string, int> termDocumentAndFrequency = new Dictionary<string, int>();
            var indexTermNotClean = Regex.Replace(content, @"[^\w\s]", string.Empty).Split(new char[] { '\r', '\n', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> indexTermDocument = indexTermNotClean.Distinct().Where(e => !StopAnalyzer.ENGLISH_STOP_WORDS_SET.Contains(e)).ToList();
            //List<string> indexTermDocument = indexTermNotClean.Distinct().ToList();

            //frequency
            foreach (var item in indexTermDocument)
            {
                int wordCountOfIndexTerm = 0;
                wordCountOfIndexTerm = Regex.Matches(content, item).Count;
                termDocumentAndFrequency.Add(item, wordCountOfIndexTerm);
            }
            indexTermList = indexTermList.Union(indexTermDocument).ToList();
            return termDocumentAndFrequency;
        }
    }
}
