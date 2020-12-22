using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxAnalysisWithSymbolTableWPF
{
    class SolutionItem
    {
        private string input;
        private Stack<string> methods;
        private List<string> steps;

        public string Input { get; set; }
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

        public SolutionItem()
        {
            this.input = "";
            this.methods = new Stack<string>();
            this.methods.Push("E");
            this.steps = new List<string>();
        }

        public SolutionItem(string input) : this() 
        {
            this.input = input;
        }

        public SolutionItem(string input, Stack<string> methods, List<string> steps) : this(input)
        {
            this.methods = methods;
            this.steps = steps;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", input, methods.ToString(), stepsToString());
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
    }
}
