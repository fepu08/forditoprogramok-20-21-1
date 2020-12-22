using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyntaxAnalysisWithSymbolTableWPF
{
    class SyntaxAnalyzerAutomat
    {
        private string original;
        private string converted;
        private int i;
        private Stack<string> stack;


        public string Original
        {
            get { return original; }
            set { original = value; }
        }

        public string Converted 
        {
            get { return converted; }
            set { converted = value; }
        }

        public Stack<string> Stack 
        {
            get { return stack; }
            set { stack = value; }
        }

        public SyntaxAnalyzerAutomat(string original)
        {
            this.i = 0;
            this.original = original;
            this.converted = $"{Simple(original)}#";
            this.stack = new Stack<string>();
            stack.Push("E");
        }

        public string Simple(string input)
        {
            // Az összes számjegyet kicseréli i betűre
            return Regex.Replace(input, "[0-9]+", "i");
        }

        public void automaton()
        {

        }
    }
}
