using forditoprogramok.finitAutomaton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forditoprogramok
{
    class Program
    {
        static void Main(string[] args)
        {
            /* #1 óra (2020.10.07.)
                StreamReader SR = new StreamReader(File.OpenRead("filepath1"));
                string s = SR.ReadToEnd(); // ezen a ponton már felülírható a file tartalma
                SR.Close();

                StreamWriter SW = new StreamWriter(File.Open("filepath2", FileMode.Create));
                SW.Flush();
                SW.Close();
            */

            //SourceHandler SH = new SourceHandler(@"C:\RandomPath\FileToRead.txt", @"C:\RandomPath\FileToWrite.txt");
            // Ezzel a relatív útvonallal a ...valami/forditoprogramok/forditoprogramok/bin/ -be localizalja a fileokat
            string readPath = "./Source.txt";
            string writePath = "./FinalCode.txt";
            string dictionaryPath = "./Dictionary.txt";

            SourceHandler SH = new SourceHandler(readPath, writePath, dictionaryPath);
            SH.openFileToRead();    // Ha nem sikerül neki kiírja a kapott hibaüzenetet a console-ba
            SH.replaceContent();
            SH.openFileToWrite();
            
            var blockComment = "\n\n/* block comment \n" +
                "alma" +
                "*/\n" +
                "//line  comment \n" +
                "//     line comment\n" +
                "IF (a == 2) {b=6}";
            SH.Content = blockComment;
            //10 20 VAR[a] 30 CONST[6] 40...
            //10 20 001 30 002 40...
            //Szimbólum tábla -> ["a", "20"]
            Console.WriteLine("Eredeti szöveg: " + blockComment);

            SH.replaceContent();
            Console.WriteLine("Result: " + SH.Content);


            Automaton A = new Automaton();
            A.Input = "+12";
            A.main();
            A.Input = "+micimacko";
            A.main();

            Console.ReadKey();
        }
    }
}
