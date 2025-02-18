namespace Semantica
{
    // Errores personalizados
    // base manda a llamar el constructor de la super clase
    public class Error : Exception
    {
        private static int line = 1;
        private static bool wasNewLine;
        private static int column = 0;
        private static int lastColumn = 0;
        public static int Line {
            get => line;
            set => line = value;       
        }
        public static bool WasNewLine {
            get => wasNewLine;
            set => wasNewLine = value;
        }
        public static int Column {
            get => column;
            set => column = value;
        }
        public static int LastColumn {
            get => lastColumn;
            set => lastColumn = value;
        }
        public Error(string message) 
            : base($"Error - {message} en la linea {line}, columna {column}") 
        {

        }

        /*public Error(string message, StreamWriter logger) 
            : base($"Error de {message}")
        {
            logger.WriteLine($"Error de {message}");
        }*/

        /*public Error(string message, StreamWriter logger) 
            : base($"Error de {message} en la linea {line}")
        {
            logger.WriteLine($"Error de {message} en la linea {line}");
        }*/

        public Error(string message, StreamWriter logger)
            : base($"Error - {message} en la linea {line}, columna {column}") 
        {
            logger.WriteLine($"Error {message} en la linea {line}, columna {column}");
        }
    }
}