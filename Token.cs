using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

        /*public void setContent(string content) {
            this.content = content;
        }

        public void setClasification(Tipos clasification) {
            this.clasification = clasification;
        }

        public string getContent() {
            return content;
        }

        public Tipos getClasification() {
            return clasification;
        }*/
    }
}