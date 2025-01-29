using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Semantica
{
    // Errores personalizados
    // base manda a llamar el constructor de la super clase
    public class Error : Exception
    {
        public Error(string message) 
            : base($"Error de {message}") 
        {

        }
        public Error(string message, StreamWriter logger) 
            : base($"Error de {message}")
        {
            logger.WriteLine($"Error de {message}");
        }

        public Error(string message, StreamWriter logger, int line) 
            : base($"Error de {message} en la linea {line}")
        {
            logger.WriteLine($"Error de {message} en la linea {line}");
        }

        public Error(string message, StreamWriter logger, int line, int column)
            : base($"Error de {message} en la linea {line}, columna {column}") 
        {
            logger.WriteLine($"Error de {message} en la linea {line}, columna {column}");
        }
    }
}