using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    REQUERIMIENTOS
    1. Implementar set y get para la clase Token
    2. Implementar parámetros por default en el constructor del archivo Léxico
*/

namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        Stack<float> s; //Evaluar expresiones
        List<Variable> l; //Lista de variables
        public Lenguaje() : base()
        {
            s = new Stack<float>();
            l = new List<Variable>();
        }

        public Lenguaje(string name) : base(name)
        {
            s = new Stack<float>();
            l = new List<Variable>();
        }

        private void displayStack()
        {
            Console.WriteLine("Contenido del Stack");
            foreach (float elemento in s)
            {
                Console.WriteLine(elemento);
            }
        }

        private void displayList()
        {
            logger.WriteLine("Lista de Variables");
            foreach (Variable elemento in l)
            {
                logger.WriteLine($"{elemento.getNombre()} {elemento.getTipoDato()} {elemento.getValor()}");
            }
        }
        // ? Cerradura epsilon
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (Content == "using")
            {
                Librerias();
            }

            if (Clasification == Tipos.TipoDato)
            {
                Variables();
            }

            Main();
            displayList();
        }

        //Librerias -> using ListaLibrerias; Librerias?
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");

            if (Content == "using")
            {
                Librerias();
            }
        }

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato t = Variable.TipoDato.Char;

            switch (Content)
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }

            match(Tipos.TipoDato);
            ListaIdentificadores(t);
            match(";");

            if (Clasification == Tipos.TipoDato)
            {
                Variables();
            }
        }

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Indentificador);

            if (Content == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }

        // ListaIdentificadores -> identificador (=Expresion)? (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(variable => variable.getNombre() == Content) != null)
            {
                throw new Error("Sintaxis: la variable " + Content + " ya existe", logger, line, column);
            }

            Variable v = new(t, Content);
            l.Add(v);

            Asignacion(v);

            if (Content == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        // BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool execute)
        {
            match("{");
            if (Content != "}")
            {
                ListaInstrucciones(execute);
            }
            match("}");
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool execute)
        {
            Instruccion(execute);

            if (Content != "}")
            {
                ListaInstrucciones(execute);
            }
        }

        // Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool execute)
        {
            if (Content == "Console")
            {
                console(execute);
            }
            else if (Content == "if")
            {
                If(execute);
            }
            else if (Content == "while")
            {
                While();
            }
            else if (Content == "do")
            {
                Do();
            }
            else if (Content == "for")
            {
                For();
            }
            else if (Clasification == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
                match(";");
            }
        }
        /*
        Agregar el resto de asignaciones:
        ID = Expresion
        ID++
        ID--
        ID IncrementoTermino Expresion
        ID IncrementoFactor Expresion
        ID = Console.Read()
        ID = Console.ReadLine()
        */
        private void Asignacion(Variable? v = null)
        {
            if (v == null)
            {
                v = l.Find(variable => variable.getNombre() == Content);

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + Content + " no está definida ", logger, line, column);
                }
            }

            match(Tipos.Indentificador);

            if (Content == "=")
            {
                match("=");

                if (Content == "Console")
                {
                    match("Console");
                    match(".");

                    if (Content == "Read")
                    {
                        match("Read");
                        int value = Console.Read();
                    }
                    else
                    {
                        match("ReadLine");
                        
                        string? read = Console.ReadLine();
                        float result;

                        if(float.TryParse(read, out result)) {
                            v.setValor(result);
                        } else {
                            throw new Error("de Sintaxis: sólo se pueden ingresar números", logger, line, column);
                        }

                        /*try
                        {
                            read = read == null ? "" : read;
                            float value = float.TryParse(read);
                            v.setValor(value);
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error);

                            throw new Error("de Sintaxis: sólo se pueden ingresar números", logger, line, column);
                        }*/
                    }

                    match("(");
                    match(")");
                }
                else
                {
                    Expresion();
                    float r = s.Pop();
                    v.setValor(r);
                }
            }
            else if (Clasification == Tipos.IncrementoTermino)
            {
                string operador = Content;
                match(Tipos.IncrementoTermino);

                if (operador == "++")
                {
                    v.setValor(v.getValor() + 1);
                }
                else if (operador == "--")
                {
                    v.setValor(v.getValor() - 1);
                }
                else
                {
                    Expresion();
                    float r = s.Pop();

                    switch (operador)
                    {
                        case "+=": v.setValor(v.getValor() + r); break;
                        case "-=": v.setValor(v.getValor() - r); break;
                    }
                }
            }
            else if (Clasification == Tipos.IncrementoFactor)
            {
                string operador = Content;
                match(Tipos.IncrementoFactor);

                Expresion();
                float r = s.Pop();

                switch (operador)
                {
                    case "*=": v.setValor(v.getValor() * r); break;
                    case "/=": v.setValor(v.getValor() / r); break;
                    case "%=": v.setValor(v.getValor() % r); break;
                }
            }
            //displayStack();
        }
        // If -> if (Condicion) bloqueInstrucciones | instruccion
        // (else bloqueInstrucciones | instruccion)?
        private void If(bool execute2)
        {
            match("if");
            match("(");

            bool execute = Condicion() && execute2;

            match(")");

            if (Content == "{")
            {
                BloqueInstrucciones(execute);
            }
            else
            {
                Instruccion(execute);
            }

            if (Content == "else")
            {
                bool executeElse = execute2 && !execute;
                match("else");

                if (Content == "{")
                {
                    BloqueInstrucciones(executeElse);
                }
                else
                {
                    Instruccion(executeElse);
                }
            }
        }
        // Condicion -> Expresion operadorRelacional Expresion
        private bool Condicion()
        {
            Expresion();
            float valor1 = s.Pop();

            string operador = Content;
            match(Tipos.OperadorRelacional);

            Expresion();
            float valor2 = s.Pop();

            switch (operador)
            {
                case ">": return valor1 > valor2;
                case ">=": return valor1 >= valor2;
                case "<": return valor1 < valor2;
                case "<=": return valor1 <= valor2;
                case "==": return valor1 == valor2;
                default: return valor1 != valor2;
            }
        }
        // While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");

            if (Content == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
        }
        // Do -> do 
        // bloqueInstrucciones | intruccion 
        // while(Condicion);
        private void Do()
        {
            match("do");

            if (Content == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }

            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        // For -> for(Asignacion; Condicion; Asignación) 
        // BloqueInstrucciones | Intruccion
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            match(";");
            Condicion();
            match(";");
            Asignacion();
            match(")");

            if (Content == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
        }
        // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console(bool execute)
        {
            bool console = false;
            string content = "";

            match("Console");
            match(".");

            switch (Content)
            {
                case "Write":
                    console = true;
                    match("Write");
                    break;
                default:
                    match("WriteLine");
                    break;
            }

            match("(");

            if (Content != ")")
            {

                /*if (Clasification == Tipos.Indentificador)
                {
                    Variable? v = l.Find(variable => variable.getNombre() == Content);

                    if (v == null)
                    {
                        throw new Error("de Sintaxis: la variable no existe", logger, line, column);
                    }

                    content += v.getValor();
                    match(Tipos.Indentificador);
                }
                else
                {
                    content += Content;
                    match(Tipos.Cadena);
                }*/

                Concatenaciones(ref content);
                
            }
            else if (console && Content == ")")
            {
                throw new Error("de Sintaxis: Se esperaba un parámetro", logger, line, column);
            }

            match(")");
            match(";");

            content = content.Replace("\"", "").Replace("\\n", "\n");

            if (execute)
            {
                if (console)
                {
                    Console.Write(content);
                }
                else
                {
                    Console.WriteLine(content);
                }
            }
        }
        // Main -> static void Main(string[] args) BloqueInstrucciones 
        private void Main()
        {
            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            BloqueInstrucciones(true);
        }
        // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        // MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            //Infix to Postfix
            if (Clasification == Tipos.OperadorTermino)
            {
                string operador = Content;
                match(Tipos.OperadorTermino);
                Termino();
                //Console.Write(operador + " ");

                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "+": s.Push(n2 + n1); break;
                    case "-": s.Push(n2 - n1); break;
                }

            }
        }
        // Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        // PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (Clasification == Tipos.OperadorFactor)
            {
                string operador = Content;
                match(Tipos.OperadorFactor);
                Factor();

                //Console.Write(operador + " ");

                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "*": s.Push(n2 * n1); break;
                    case "/": s.Push(n2 / n1); break;
                    case "%": s.Push(n2 % n1); break;
                }
            }
        }
        // Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (Clasification == Tipos.Numero)
            {
                s.Push(float.Parse(Content));
                //Console.Write(Content + " ");
                match(Tipos.Numero);
            }
            else if (Clasification == Tipos.Indentificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == Content);

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + Content + " no está definida ", logger, line, column);
                }

                s.Push(v.getValor());
                //Console.Write(Content + " ");
                match(Tipos.Indentificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }

        // Concatenaciones -> (Identificador | Cadena) (+ Concatenaciones) ?
        private void Concatenaciones(ref string content)
        {
            if (Clasification == Tipos.Indentificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == Content);

                if (v == null)
                {
                    throw new Error("de Sintaxis: la variable no existe", logger, line, column);
                }

                content += v.getValor();
                match(Tipos.Indentificador);
            }
            else
            {
                content += Content;
                match(Tipos.Cadena);
            }

            if (Content == "+")
            {
                match("+");
                Concatenaciones(ref content);
            }
        }
    }
}