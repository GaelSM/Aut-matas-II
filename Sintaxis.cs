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
                Error.LastColumn = Error.Column;
                NextToken();
            }
            else
            {
                if(Error.WasNewLine) {
                    Error.Column = Error.LastColumn + 1;
                    Error.Line--;
                }

                throw new Error("Sintaxis: se espera un " + content, logger);
            }
        }
        //Comparar tipo específico
        public void match(Tipos clasification)
        {
            if (clasification == Clasification)
            {
                Error.LastColumn = Error.Column;
                NextToken();
            }
            else
            {
                if(Error.WasNewLine) {
                    Error.Column = Error.LastColumn + 1;
                    Error.Line--;
                }
                
                throw new Error("Sintaxis: se espera un " + clasification, logger);
            }
        }
    }
}