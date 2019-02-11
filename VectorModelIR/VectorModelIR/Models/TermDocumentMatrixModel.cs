using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorModelIR
{
    public class TermDocumentMatrixModel
    {
        public Dictionary<string, Dictionary<string, double>> TFIndexTermOfDocument { get; set; }
        public Dictionary<string, double> IDFIndexTermOfDocument { get; set; }
        public Dictionary<string, Dictionary<string, double>> TF_IDFIndexTermOfDocument { get; set; }
        public Dictionary<string, int> IsTermInDocument { get; set; }
        public List<string> IndexTermList { get; set; }
        public List<DocumentModel> DocumentList { get; set; }
    }
}
