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
        private Stack<string> methods;
        private List<string> steps;


        public string Original
        {
            get { return original; }
            set 
            { 
                original = value;
                this.converted = Simple(original);
                if (this.converted.Substring(  this.converted.Length - 1) != "#") { this.converted += "#"; }
            }
        }

        public string Converted 
        {
            get { return converted; }
            set 
            {
                converted = value;
                if(converted.Substring(converted.Length - 1) != "#") converted += "#";
            }
        }

        public Stack<string> Methods
        {
            get
            {
                return new Stack<string>(new Stack<string>(methods));
            }
            set
            {
                this.methods = value;
            }
        }

        public List<string> Steps { get; set; }

        public SyntaxAnalyzerAutomat()
        {
            this.i = 0;
            this.original = "";
            this.converted = "";
            this.methods = new Stack<string>();
            methods.Push("E");
            this.steps = new List<string>();
        }

        public SyntaxAnalyzerAutomat(string original) : this()
        {
            this.i = 0;
            this.original = original;
            this.converted = Simple(original);
            if(this.converted.Substring(this.converted.Length - 1) != "#") { this.converted += "#"; }
        }

        public string GetSolution()
        {
            return String.Format("({0}, {1}, {2})", converted, MethodsToString(), stepsToString());
        }

        public string Simple(string input)
        {
            // Az összes számjegyet kicseréli i betűre
            return Regex.Replace(input, "[0-9]+", "i");
        }

        private string stepsToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < steps.Count; i++)
            {
                sb.Append(steps[i]);
            }
            return sb.ToString();
        }

        private string MethodsToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string method in methods)
            {
                sb.Append(method.ToString());
            }
            return sb.ToString();
        }

    }
}
