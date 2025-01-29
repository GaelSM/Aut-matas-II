using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semantica
{
    public class Sintaxis : Lexico
    {
        public Sintaxis() : base()
        {
            NextToken();
        }

        public Sintaxis(string name) : base(name)
        {
            NextToken();
        }
        //Comparar texto
        public void match(string content)
        {
            if (content == Content)
            {
                lastColumn = column;
                NextToken();
            }
            else
            {
                if(wasNewLine) {
                    column = lastColumn + 1;
                    line--;
                }

                throw new Error("Sintaxis: se espera un " + content, logger, line, column);
            }
        }
        //Comparar tipo específico
        public void match(Tipos clasification)
        {
            if (clasification == Clasification)
            {
                lastColumn = column;
                NextToken();
            }
            else
            {
                if(wasNewLine) {
                    column = lastColumn + 1;
                    line--;
                }
                
                throw new Error("Sintaxis: se espera un " + clasification, logger, line, column);
            }
        }
    }
}