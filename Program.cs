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
                Token t = new()
                {
                    Content = ">=",
                    Clasification = Tipos.OperadorRelacional
                };

                Console.WriteLine(t.Content);
                Console.WriteLine(t.Clasification);

                /*Lexico l = new Lexico();
                l.GetAllTokens();*/
                /*using Lenguaje l = new("prueba.cpp");
                l.Programa();*/
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }
}