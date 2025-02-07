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
                Error.lastColumn = Error.column;
                NextToken();
            }
            else
            {
                if(Error.wasNewLine) {
                    Error.column = Error.lastColumn + 1;
                    Error.line--;
                }

                throw new Error("Sintaxis: se espera un " + content, logger);
            }
        }
        //Comparar tipo específico
        public void match(Tipos clasification)
        {
            if (clasification == Clasification)
            {
                Error.lastColumn = Error.column;
                NextToken();
            }
            else
            {
                if(Error.wasNewLine) {
                    Error.column = Error.lastColumn + 1;
                    Error.line--;
                }
                
                throw new Error("Sintaxis: se espera un " + clasification, logger);
            }
        }
    }
}