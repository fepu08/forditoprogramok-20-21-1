using System;
using System.Collections.Generic;
using System.IO;

namespace forditoprogramok.finitAutomaton
{
    class Automaton
    {
        /*TODO: 2020.11.9. 
         * ne csak egy elemre mondja meg hogy helyes-e vagy nem
         * hanem be kell olvasni egy file-t ami szamokat tartalmaz szokozokkel evalasztva
         * +233 +24 34 01 ... 
         * Szét kell darabolni a szokozok menten a  metodussal es belekell rakni egy string listaba
         * List<String> s = String.split(" ")
         * minden ilyen elemre (s elemeire) meg kell hívni a main-t
        */

        private string state;
        private string inputPath;
        private List<String> inputList; //Values to be examined
        private static string error = "Error";
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        public Automaton()
        {
            this.state = "A";
            this.inputPath = "./automata-source.txt"; //keresse itt, ha nem adjuk meg
            this.inputList = new List<string>();
            PrepareDictionary();
            ReadValuesFromInputFile();
        }

        public Automaton(string inputPath) : this()
        {
            this.inputPath = inputPath;
        }

        public string State { get;}

        public string InputPath 
        {
            get
            {
                return inputPath;
            }
            set 
            {
                this.inputPath = value;
                ReadValuesFromInputFile();
            } 
        }

        public List<String> InputList 
        {
            get
            {
                List<string> temp = new List<string>();
                for (int i = 0; i < inputList.Count; i++)
                {
                    temp.Add(inputList[i]);
                }
                return temp;
            }
            set
            {
                inputList.Clear();
                for (int i = 0; i < value.Count; i++)
                {
                    inputList.Add(value[i]);
                }
            }
        }

        public bool ReadValuesFromInputFile()
        {
            List<String> temp = new List<string>();
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(inputPath));
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] words = line.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        temp.Add(words[i]);
                    }
                }
                sr.Close();
            }
            catch (Exception IOE)
            {
                Console.WriteLine(IOE.Message);
                return false;
            }
            InputList = temp;
            return true;
        }

        public void PrepareDictionary()
        {
            dictionary.Add("A+", "B");
            dictionary.Add("A-", "B");
            dictionary.Add("Ad", "C");
            dictionary.Add("Bd", "C");
            dictionary.Add("Cd", "C");
        }

        public char Convert(char c)
        {
            if (Char.IsDigit(c))
            {
                return 'd';
            }
            return c;
        }

        public void ResetState()
        {
            this.state = "A";
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

        public string Delta(string str, char act)
        {
            string ex = str + Convert(act);
            if (dictionary.ContainsKey(ex))
            {
                // ha megtalaljuk vissza adjuk a kulcs altal mutatott value-t
                return dictionary[ex];
            }
            return error;
        }

        // automata megvalositasa
        public void Main()
        {
            /*int i = 0;
            while (i < inputPath.Length && state != error)
            {
                state = delta(state, inputPath[i]);
                i++; 
            }

            if(state != error)
            {
                Console.WriteLine("{0} helyes bemenő adat", inputPath);
            } else
            {
                Console.WriteLine(
                    "{0} nem helyes bemenő adat. Hibás karakter található a {1}. helyen",
                    this.inputPath, i);
            }*/

            foreach (string value in inputList)
            {
                ResetState();
                int i = 0;
                while (i < value.Length && state != error)
                {
                    state = Delta(state, value[i]);
                    i++;
                }

                // FONTOS hogy csak a C állapot az elfogadó állapot
                // Ezért a feltétel lehetne az is, hogy if(state == "C")
                if (state != error && state != "B")
                {
                    Console.WriteLine("{0} : helyes bemenő adat", value);
                }
                else
                {
                    Console.WriteLine(
                        "{0} : NEM helyes bemenő adat. Hibás karakter található a(z) {1}. helyen",
                        value, i);
                }
            }
        }
    }
}
