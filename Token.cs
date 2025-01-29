using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

/*
    DESCRIPTORES DE ACCESO
    Una propiedad sin un descriptor de acceso set se considera de solo lectura. 
    Una propiedad sin un descriptor de acceso get se considera de solo escritura.
    Una propiedad que tiene ambos descriptores de acceso es de lectura y escritura. 
    
    GET
    El bloque de código del descriptor de acceso get se ejecuta cuando se lee la propiedad.

    SET
    El bloque de código del descriptor de acceso set oinit se ejecuta cuando se asigna un 
    valor a la propiedad.Usa un parámetro implícito denominado value, 
    cuyo tipo es el tipo de la propiedad. 

    Las propiedades tienen muchos usos:

    Pueden validar los datos antes de permitir un cambio.
    Pueden exponer datos de forma transparente en una clase donde esos datos se recuperan de algún otro origen, como una base de datos.
    Pueden realizar una acción cuando se cambian los datos, como generar un evento o cambiar el valor de otros campos.
*/

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