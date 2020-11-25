using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forditoprogramok
{
    /**
     * Source Handler
     * Olyan osztály ami képes arra, hogy:
     *  - fileokat nyisson meg
     *  - később tudja módosítani a beolvasott file tartalmat
     *  - ezt ki tudja írni, egy általunk meghatározott fileba
     */
    class SourceHandler
    {
        /** Tartalma:
         *  - metódus: betudunk olvasni tartalmat egy fileba
         *  - metódus: ki tudunk írni tartalmat egy fileba
         *  - metódus: amivel a beolvasott file tartalmát tudjuk változtatni
         *  - konstruktor: amiben meghatározzuk, hogy milyen fileokat kezelünk (megnyitásra és írásra szánt file)
         *  - propertyk hogy ezeket a file neveket(olvasni és írni szánt fileokat) az osztály megnyitása után is tudjuk változtatni
         */

        // eredetileg path1, és path2 néven lettek megírva órán
        private string filePathToRead, filePathToWrite, dictionaryPath = "";   // file nevek tárolására
        private string content = "";  // a beolvasott file tartalmát tároljuk
        private Dictionary<string, string> replacesDictionary = new Dictionary<string, string>();
        private List<string> symbolTable = new List<string>();
        private int symbolIndex = 0;
        public string FilePathToRead 
        {
            get { return filePathToRead; }
            set 
            {
                if (true)
                {
                    filePathToRead = value; 
                } else
                {
                    throw new Exception("Hibás érték");
                }
            }
        }

        public string FilePathToWrite
        {
            get { return filePathToWrite; }
            set
            {
                if (true)
                {
                    filePathToWrite = value;
                }
                else
                {
                    throw new Exception("Hibás érték");
                }
            }
        }

        public string Content
        {
            get { return content; }
            set
            {
                if (true)
                {
                    content = value;
                }
                else
                {
                    throw new Exception("Hibás érték");
                }
            }
        }

        public SourceHandler(string filePathToRead, string filePathToWrite, string dictionaryPath)
        {
            this.filePathToRead = filePathToRead;
            this.filePathToWrite = filePathToWrite;
            this.dictionaryPath = dictionaryPath;
            this.readDictionary();
        }

        public bool openFileToRead()
        {
            try
            {
                StreamReader SR = new StreamReader(File.OpenRead(filePathToRead));
                content = SR.ReadToEnd();
                SR.Close();
            }
            catch(Exception IOE)
            {
                Console.WriteLine(IOE.Message);
                return false;
            }
            return true;
        }

        public bool openFileToWrite()
        {
            try
            {
                StreamWriter SW = new StreamWriter(File.Open(filePathToWrite, FileMode.Create));
                SW.WriteLine(this.content); //testing
                SW.Flush();
                SW.Close();
            }
            catch (Exception IOE)
            {
                Console.WriteLine(IOE.Message);
                return false;
            }
            return true;
        }

        private void readDictionary()
        {
            try
            {
                string line = "";
                StreamReader SR = new StreamReader(File.OpenRead(dictionaryPath));
                while ((line = SR.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    replacesDictionary.Add(words[0], words[1]);
                }
                SR.Close();
            }
            catch (Exception IOE)
            {
                Console.WriteLine(IOE.Message);
            }

        }


        /** replaceContent(s)
         * A megfelelő módon majd felülírjuk a content változó tartalmát
         */
        public void replaceContent()
        {
            //var blockComments = @"/\*(.*?)\*/";
            //var blockComments = @"^/[/|*](.+)$";
            //var lineComments = @"//(.*?)\r?\n";
            string patternBlockComment = @"/[*][\w\d\s]+[*]/";
            string patternLineComment = @"//.*?\n";
            string patternNumber = @"([0-9]+)";
            string patternVar = @"([a-z-_]+)";
            string replaceNumber = " CONST[$1] ";
            string replaceVar = " VAR[$1] ";

            content = Regex.Replace(content, patternBlockComment, String.Empty);
            content = Regex.Replace(content, patternLineComment, String.Empty);
            content = Regex.Replace(content, "\r", String.Empty);
            content = Regex.Replace(content, patternNumber, changeVariablesAndConstants("$1"));
            content = Regex.Replace(content, patternVar, changeVariablesAndConstants("$1"));

            foreach (var x in replacesDictionary)
            {
                while (content.Contains(x.Key))
                {
                    content = content.Replace(x.Key, x.Value);
                }
            }
        }

        /* KIINDULÁS - beolvasása openFileToRead() metódussal
         * int       i =   10; // asdlkhawpihn
         * while (i < 10 ) {
         *  i++;
         * }
         */
        //"int i=10;while(i<10){i++;}" ehhez hasonló VÉGEREDMÉNY

        string changeVariablesAndConstants(string varAndConstName)
        {
            symbolTable.Add(varAndConstName);
            symbolIndex++;
            string result = "00" + symbolIndex.ToString();
            return result.Substring(result.Length - 3);
        }

        /** replaceText() - Feladata, hogy kicseréljen bármilyen szöveget bármire*/
        public void replaceText(string from, string to)
        {
            while (content.Contains(from))
            {
                content = content.Replace(from, to);
            }
        }
        
        public void replaceFirst()
        {
            //content = Replace(content, @"[a-zA-Z0-9]", "VARIABLE");
            //content = Regex.Replace(content, @"[0-9]", "CONST");
            //TODO: betenni egy fileba és soronként feldolgozni
            //replacesDictionary.Add("  ", " ");
            //replacesDictionary.Add("\n", " ");
            //replacesDictionary.Add("    ", " ");
            //replacesDictionary.Add(" {", "{");
            //replacesDictionary.Add(" }", "}");
            //replacesDictionary.Add("} ", "}");
            //replacesDictionary.Add("{ ", "{");
            //replacesDictionary.Add(" (", "(");
            //replacesDictionary.Add("( ", "(");
            //replacesDictionary.Add(" )", ")");
            //replacesDictionary.Add(") ", ")");
            //replacesDictionary.Add(" ;", ";");
            //replacesDictionary.Add("; ", ";");
            //replacesDictionary.Add(" =", "=");
            //replacesDictionary.Add("= ", "=");
            //replacesDictionary.Add("if", " 10 ");
            //replacesDictionary.Add("else", " 11 ");
            //replacesDictionary.Add("for", " 20 ");
            //replacesDictionary.Add("while", " 21 ");
            //replacesDictionary.Add("switch", " 30 ");
            //replacesDictionary.Add("case", " 31 ");
            //replacesDictionary.Add("(", " 40 ");
            //replacesDictionary.Add(")", " 41 ");
            //replacesDictionary.Add("==", " 50 ");
            //replacesDictionary.Add(">", " 51 ");
            //replacesDictionary.Add(">=", " 52 ");
            //replacesDictionary.Add("<", " 53 ");
            //replacesDictionary.Add("<=", " 54 ");
            //replacesDictionary.Add("{", " 60 ");
            //replacesDictionary.Add("}", " 61 ");
            //replacesDictionary.Add("=", " 70 ");
            //replacesDictionary.Add("+", " 71 ");
            //replacesDictionary.Add("++", " 72 ");
            //replacesDictionary.Add("-", " 73 ");
            //replacesDictionary.Add("--", " 74 ");
            //replacesDictionary.Add("-=", " 75 ");
            //replacesDictionary.Add("+=", " 76 ");
            //replacesDictionary.Add("true", " 80 ");
            //replacesDictionary.Add("false", " 81 ");
            //foreach (KeyValuePair<string, string> kvp in replacesDictionary)
            //{
            //    replaceText(kvp.Key, kvp.Value);
            //}
        }
    }
}
