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
        public void match(string content)
        {
            if (content == getContent())
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

        public void match(Tipos clasification)
        {
            if (clasification == getClasification())
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