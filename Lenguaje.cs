using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    1. Concatenaciones (LISTO)
    2. Inicializar una variable desde la declaración (LISTO)
    3. Evaluar las expresiones matemáticas (LISTO)
    4. Levantar si en el Console.ReadLine() no ingresan números (LISTO)
    5. Modificar la variable con el resto de operadores (Incremento de factor y termino) (LISTO)
    6. Hacer que funcione el if/else
*/

namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        Stack<float> s;
        List<Variable> l;
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
            if (getContent() == "using")
            {
                Librerias();
            }

            if (getClasification() == Tipos.TipoDato)
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

            if (getContent() == "using")
            {
                Librerias();
            }
        }

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato t = Variable.TipoDato.Char;

            switch (getContent())
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }

            match(Tipos.TipoDato);
            ListaIdentificadores(t);
            match(";");

            if (getClasification() == Tipos.TipoDato)
            {
                Variables();
            }
        }

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Indentificador);

            if (getContent() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }

        // ListaIdentificadores -> identificador (=Expresion)? (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(variable => variable.getNombre() == getContent()) != null)
            {
                throw new Error("Sintaxis: la variable " + getContent() + " ya existe", logger, line, column);
            }

            Variable v = new(t, getContent());
            l.Add(v);

            Asignacion(v);

            if (getContent() == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        // BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool execute)
        {
            match("{");
            if (getContent() != "}")
            {
                ListaInstrucciones(execute);
            }
            match("}");
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool execute)
        {
            Instruccion(execute);

            if (getContent() != "}")
            {
                ListaInstrucciones(execute);
            }
        }

        // Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool execute)
        {
            if (getContent() == "Console")
            {
                console(execute);
            }
            else if (getContent() == "if")
            {
                If(execute);
            }
            else if (getContent() == "while")
            {
                While();
            }
            else if (getContent() == "do")
            {
                Do();
            }
            else if (getContent() == "for")
            {
                For();
            }
            else if (getClasification() == Tipos.TipoDato)
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
                v = l.Find(variable => variable.getNombre() == getContent());

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + getContent() + " no está definida ", logger, line, column);
                }
            }

            match(Tipos.Indentificador);

            if (getContent() == "=")
            {
                match("=");

                if (getContent() == "Console")
                {
                    match("Console");
                    match(".");

                    if (getContent() == "Read")
                    {
                        match("Read");
                        int value = Console.Read();
                    }
                    else
                    {
                        match("ReadLine");
                        string? read = Console.ReadLine();

                        try
                        {
                            read = read == null ? "" : read;
                            float value = float.Parse(read);
                            v.setValor(value);
                        }
                        catch (Exception)
                        {
                            throw new Error("de Sintaxis: sólo se pueden ingresar números", logger, line, column);
                        }
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
            else if (getClasification() == Tipos.IncrementoTermino)
            {
                string operador = getContent();
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
            else if (getClasification() == Tipos.IncrementoFactor)
            {
                string operador = getContent();
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

            if (getContent() == "{")
            {
                BloqueInstrucciones(execute);
            }
            else
            {
                Instruccion(execute);
            }

            if (getContent() == "else")
            {
                bool executeElse = execute2 && !execute;
                match("else");

                if (getContent() == "{")
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

            string operador = getContent();
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

            if (getContent() == "{")
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

            if (getContent() == "{")
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

            if (getContent() == "{")
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

            switch (getContent())
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

            if (getContent() != ")")
            {

                /*if (getClasification() == Tipos.Indentificador)
                {
                    Variable? v = l.Find(variable => variable.getNombre() == getContent());

                    if (v == null)
                    {
                        throw new Error("de Sintaxis: la variable no existe", logger, line, column);
                    }

                    content += v.getValor();
                    match(Tipos.Indentificador);
                }
                else
                {
                    content += getContent();
                    match(Tipos.Cadena);
                }*/

                Concatenaciones(ref content);
                
            }
            else if (console && getContent() == ")")
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
            if (getClasification() == Tipos.OperadorTermino)
            {
                string operador = getContent();
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
            if (getClasification() == Tipos.OperadorFactor)
            {
                string operador = getContent();
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
            if (getClasification() == Tipos.Numero)
            {
                s.Push(float.Parse(getContent()));
                //Console.Write(getContent() + " ");
                match(Tipos.Numero);
            }
            else if (getClasification() == Tipos.Indentificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == getContent());

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + getContent() + " no está definida ", logger, line, column);
                }

                s.Push(v.getValor());
                //Console.Write(getContent() + " ");
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
            if (getClasification() == Tipos.Indentificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == getContent());

                if (v == null)
                {
                    throw new Error("de Sintaxis: la variable no existe", logger, line, column);
                }

                content += v.getValor();
                match(Tipos.Indentificador);
            }
            else
            {
                content += getContent();
                match(Tipos.Cadena);
            }

            if (getContent() == "+")
            {
                match("+");
                Concatenaciones(ref content);
            }
        }
    }
}