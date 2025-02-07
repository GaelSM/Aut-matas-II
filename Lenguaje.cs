/*
    REQUERIMIENTOS
    1. Implementar set y get para la clase Token (LISTO)
    2. Implementar parámetros por default en los constructores del archivo Léxico (LISTO)
    3. Implementar línea y columna en los Errores Semánticos (LISTO)
    4. Implementar máximoTipo en la asignación, es decir, cuando se haga v.setValor(r) (LISTO)
    5. Aplicar el casteo en el stack (LISTO)
*/

namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        Stack<float> s; //Evaluar expresiones
        List<Variable> l; //Lista de variables
        Variable.TipoDato maximoTipo;
        public Lenguaje() : base()
        {
            s = new Stack<float>();
            l = new List<Variable>();
            maximoTipo = Variable.TipoDato.Char;
        }

        public Lenguaje(string name) : base(name)
        {
            s = new Stack<float>();
            l = new List<Variable>();
            maximoTipo = Variable.TipoDato.Char;
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
                throw new Error("Sintaxis: la variable " + Content + " ya existe", logger);
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
            //Cada vez que haya una asignación reiniciar el maximo tipo
            maximoTipo = Variable.TipoDato.Char;

            if (v == null)
            {
                v = l.Find(variable => variable.getNombre() == Content);

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + Content + " no está definida ", logger);
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
                            throw new Error("de Sintaxis: sólo se pueden ingresar números", logger);
                        }
                    }

                    match("(");
                    match(")");
                }
                else
                {
                    Expresion();

                    float r = s.Pop();

                    //Console.WriteLine("Tipo de la variable: " + v.getTipoDato());
                   // Console.WriteLine("Tipo de dato máximo:" + maximoTipo);

                    if(v.getTipoDato() < maximoTipo) {
                        throw new Error("Semántico: no se puede asignar un " + maximoTipo + " a un " + v.getTipoDato());
                    }

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
            maximoTipo = Variable.TipoDato.Char;
            Expresion();
            float valor1 = s.Pop();

            string operador = Content;
            match(Tipos.OperadorRelacional);

            maximoTipo = Variable.TipoDato.Char;
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
                Concatenaciones(ref content);   
            }
            else if (console && Content == ")")
            {
                throw new Error("Sintaxis: Se esperaba un parámetro", logger);
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
                //Si el tipo de dato del número es mayor al tipo de dato actual, cambiarlo
                if(maximoTipo < Variable.valorToTipoDato(float.Parse(Content))) {
                    maximoTipo =  Variable.valorToTipoDato(float.Parse(Content));
                }

                s.Push(float.Parse(Content));
                match(Tipos.Numero);
            }
            else if (Clasification == Tipos.Indentificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == Content);

                if (v == null)
                {
                    throw new Error("Sintaxis: la variable " + Content + " no está definida ", logger);
                }

                if(maximoTipo < v.getTipoDato()) {
                    maximoTipo = v.getTipoDato();
                }

                s.Push(v.getValor());
                match(Tipos.Indentificador);
            }
            else
            {
                match("(");

                Variable.TipoDato tipoCasteo = Variable.TipoDato.Char;
                bool huboCasteo = false;

                if(Clasification == Tipos.TipoDato) {
                    
                    switch(Content) {
                        case "int": tipoCasteo = Variable.TipoDato.Int; break;
                        case "float": tipoCasteo = Variable.TipoDato.Float; break;
                    }

                    match(Tipos.TipoDato);
                    match(")");
                    match("(");

                    huboCasteo = true;
                }

                Expresion();

                if(huboCasteo) {
                    maximoTipo = tipoCasteo;

                    float castedValue = s.Pop();
                    castedValue %= Variable.getMaxValueByType(maximoTipo) + 1;
                    s.Push(castedValue);
                }

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
                    throw new Error("de Sintaxis: la variable no existe", logger);
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