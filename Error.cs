namespace Semantica
{
    // Errores personalizados
    // base manda a llamar el constructor de la super clase
    public class Error : Exception
    {
        public static int line = 1;
        public static bool wasNewLine;
        public static int column = 0;
        public static int lastColumn = 0;
        
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