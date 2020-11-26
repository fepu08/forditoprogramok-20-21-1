using System;
using System.Collections.Generic;

namespace forditoprogramok.finitAutomaton
{
    class Automaton
    {
        private string state;
        private string input;
        private static string error = "Error";
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public Automaton()
        {
            this.state = "A";
            prepareDictionary();
        }

        public Automaton(string input) : this()
        {
            this.input = input;
        }

        public string State { get;}

        public string Input 
        {
            get
            {
                return input;
            }
            set 
            {
                this.state = "A";
                this.input = value;
            } 
        }

        public void prepareDictionary()
        {
            dictionary.Add("A+", "B");
            dictionary.Add("A-", "B");
            dictionary.Add("Ad", "C");
            dictionary.Add("Bd", "C");
            dictionary.Add("Cd", "C");
        }

        public char convert(char c)
        {
            if (Char.IsDigit(c))
            {
                return 'd';
            }
            return c;
        }


        /*public string delta(string str, char act)
        {
            string ex = str + convert(act);
            switch (ex)
            {
                case "A+": return "B";
                case "A-": return "B";
                case "Ad": return "C";
                case "Bd": return "C";
                case "Cd": return "C";
                default: return error;
            }
        }*/

        public string delta(string str, char act)
        {
            string ex = str + convert(act);
            if (dictionary.ContainsKey(ex))
            {
                // ha megtalaljuk vissza adjuk a kulcs altal mutatott value-t
                return dictionary[ex];
            }
            return error;
        }

        // automata megvalositasa
        public void main()
        {
            int i = 0;
            while (i < input.Length && state != error)
            {
                state = delta(state, input[i]);
                i++; 
            }

            if(state != error)
            {
                Console.WriteLine("{0} helyes bemenő adat", input);
            } else
            {
                Console.WriteLine(
                    "{0} nem helyes bemenő adat. Hibás karakter található a {1}. helyen",
                    this.input, i);
            }
        }
    }
}
