using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace forditoprogramok
{
    class SyntaxAnalyzerAutomat
    {
        string input;
        int i;

        public SyntaxAnalyzerAutomat(string input)
        {
            this.i = 0;
            Console.WriteLine("Eredeti input: " + input);
            this.input = $"{Simple(input)}#";
            Console.WriteLine("Az egyszerűsített input: " + this.input);
        }

        public string Input
        {
            get
            {
                return this.input;
            }
            set
            {
                this.input = value;
                Simple(this.input);
                this.i = 0;
            }
        }

        public string Simple(string input)
        {
            // Az összes számjegyet kicseréli i betűre
            return Regex.Replace(input, "[0-9]+", "i");
        }

        private void Elfogad(char ch)
        {
            if (input[i] != ch)
            {
                Console.WriteLine("Hibás kifejezés {0}. Helytelen karakter: {1}", input, input[i]);
                i++;
                S();
            }
            // Ha az inkrementálás nem egy else ágon belül van
            // akkor IndexOutOfRangeExceptiont fog dobni (pl "(12*3+123" ebben )
            // Ha viszont nem, akkor meg 
            else i++;
        }

        public void S()
        {
            E();
            Elfogad('#');
            Console.WriteLine("Az elemzés lefutott");
        }

        private void E()
        {
            T();
            Ev(); // E vessző :D
        }

        private void Ev()
        {
            // +TEv | e 
            // ha az aktuális helyen + van, akkor meghívja a TEv 
            // ha nem, akkor nem kell semmit csinálni - e - epszilon
            if (input[i] == '+')
            {
                Elfogad('+');
                T();
                Ev();
            }
        }

        private void T()
        {
            F();
            Tv(); // T vessző :D
        }

        private void Tv()
        {
            // *FTv | e 
            // ha az aktuális helyen * van, akkor meghívja a FTv 
            // ha nem, akkor nem kell semmit csinálni - e - epszilon
            if (input[i] == '*')
            {
                Elfogad('*');
                F();
                Tv();
            }
        }

        private void F()
        {
            // (E) | i 
            // ha ( van, akkor meg kell hívni az elfogadot a ( -re, az E-t, és az elfogad-ot a )-re
            // különben az elfogadot az i-re használhatnánk 
            if(input[i] == '(')
            {
                Elfogad('(');
                E();
                Elfogad(')');
            } else { // if(input[i] == 'i')
                Elfogad('i');
            }
        }
    }
}
