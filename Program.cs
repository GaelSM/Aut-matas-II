using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semantica
{
    public class Program : Token
    {
        static void Main()
        {
            try
            {
                using Lenguaje l = new("prueba.cpp");
                l.Programa();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
}