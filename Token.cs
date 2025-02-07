namespace Semantica
{
    public class Token
    {
        public enum Tipos
        {
            Indentificador, 
            Numero, 
            Caracter, 
            FinSentencia,
            InicioBloque,
            FinBloque,
            OperadorTermino,
            OperadorTernario,
            OperadorFactor,
            IncrementoTermino,
            IncrementoFactor,
            Flecha,
            Asignacion,
            OperadorRelacional,
            OperadorLogico,
            Puntero,            
            Cadena,
            TipoDato,
            PalabraReservada
        }
        private string content;
        public string Content {
            get => content;
            set => content = value;
        }

        private Tipos clasification;
        public Tipos Clasification {
            get => clasification;
            set => clasification = value;
        }
        public Token()
        {
            content = "";
            clasification = Tipos.Caracter;
        }
    }
}