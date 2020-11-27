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
        private static string patternNumber = @"([0-9]+)";
        private static string patternVar = @"([a-z-_]+)";

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
                    PurgeSymbolTable();
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
            this.ReadDictionary();
        }

        public bool OpenFileToRead()
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

        public bool OpenFileToWrite()
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

        private void ReadDictionary()
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

        public void Replace()
        {
            ClearComments();
            content = Regex.Replace(content, "\n", String.Empty);
            content = Regex.Replace(content, "\r", String.Empty);
            ReplaceNumbers();
            ReplaceVariables(); // Ha a kód nincs nagybetűvel írva, azokat is változónak érzékeli (pl if -> VAR[if])
            ReplaceUsingDictionary();
            StoreVariablesAndConstants(content);
            RemoveSpaces();
        }

        public void ClearComments()
        {
            string patternBlockComment = @"/[*][\w\d\s]+[*]/";
            string patternLineComment = @"//.*?\n";
            content = Regex.Replace(content, patternBlockComment, String.Empty);
            content = Regex.Replace(content, patternLineComment, String.Empty);
        }

        public void ReplaceNumbers()
        {
            content = Regex.Replace(content, patternNumber, " CONST[$0] ");
        }

        public void ReplaceVariables()
        {
            content = Regex.Replace(content, patternVar, " VAR[$0] ");
        }

        public void ReplaceUsingDictionary()
        {
            foreach (var x in replacesDictionary)
            {
                while (content.Contains(x.Key))
                {
                    content = content.Replace(x.Key, x.Value);
                }
            }
        }

        public void StoreVariablesAndConstants(string data)
        {
            string[] contentArray = data.Split(' ');
            string tempPattern = string.Format($@"(VAR\[.*\]+|CONST\[.*\]+)");
            foreach (var symbol in contentArray)
            {
                if (Regex.IsMatch(symbol, tempPattern))
                {
                    string tempSymbol;
                    if (symbol.Contains("VAR"))
                    {
                        int start = symbol.IndexOf("VAR");
                        tempSymbol = symbol.Substring(start, symbol.IndexOf("]") - start + 1);
                    }
                    else
                    {
                        int start = symbol.IndexOf("CONST");
                        tempSymbol = symbol.Substring(start, symbol.IndexOf("]") - start + 1);
                    }

                    symbolTable.Add(tempSymbol);
                    symbolIndex += 1;
                    string response = "00" + symbolIndex.ToString();
                    content = content.Replace(tempSymbol, response.Substring(response.Length - 3));
                }
            }
        }

        public void RemoveSpaces()
        {
            while (content[0] == ' ') { content = content.Substring(1); }
            // Előfordult, hogy dupla space lett pl if(..) itt { .. ezért
            content = Regex.Replace(content, "  ", " ");
        }

        /** replaceText() - Feladata, hogy kicseréljen bármilyen szöveget bármire*/
        public void ReplaceText(string from, string to)
        {
            while (content.Contains(from))
            {
                content = content.Replace(from, to);
            }
        }

        private void PurgeSymbolTable()
        {
            symbolTable.Clear();
            symbolIndex = 0;
        }
    }
}
