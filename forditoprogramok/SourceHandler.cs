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
        private string filePathToRead, filePathToWrite = "";   // file nevek tárolására
        private string content = "";  // a beolvasott file tartalmát tároljuk
        private Dictionary<string, string> replaces = new Dictionary<string, string>();
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

        public SourceHandler(string filePathToRead, string filePathToWrite)
        {
            this.filePathToRead = filePathToRead;
            this.filePathToWrite = filePathToWrite;
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

            foreach (var x in replaces)
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
            replaces.Add("  ", " ");
            replaces.Add("\n", " ");
            replaces.Add("    ", " ");
            replaces.Add(" {", "{");
            replaces.Add(" }", "}");
            replaces.Add("} ", "}");
            replaces.Add("{ ", "{");
            replaces.Add(" (", "(");
            replaces.Add("( ", "(");
            replaces.Add(" )", ")");
            replaces.Add(") ", ")");
            replaces.Add(" ;", ";");
            replaces.Add("; ", ";");
            replaces.Add(" =", "=");
            replaces.Add("= ", "=");
            replaces.Add("if", " 10 ");
            replaces.Add("else", " 11 ");
            replaces.Add("for", " 20 ");
            replaces.Add("while", " 21 ");
            replaces.Add("switch", " 30 ");
            replaces.Add("case", " 31 ");
            replaces.Add("(", " 40 ");
            replaces.Add(")", " 41 ");
            replaces.Add("==", " 50 ");
            replaces.Add(">", " 51 ");
            replaces.Add(">=", " 52 ");
            replaces.Add("<", " 53 ");
            replaces.Add("<=", " 54 ");
            replaces.Add("{", " 60 ");
            replaces.Add("}", " 61 ");
            replaces.Add("=", " 70 ");
            replaces.Add("+", " 71 ");
            replaces.Add("++", " 72 ");
            replaces.Add("-", " 73 ");
            replaces.Add("--", " 74 ");
            replaces.Add("-=", " 75 ");
            replaces.Add("+=", " 76 ");
            replaces.Add("true", " 80 ");
            replaces.Add("false", " 81 ");
            foreach (KeyValuePair<string, string> kvp in replaces)
            {
                replaceText(kvp.Key, kvp.Value);
            }
        }
    }
}
