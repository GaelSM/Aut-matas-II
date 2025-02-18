namespace Semantica
{
    public class Variable
    {
        public const int MAX_CHAR_VALUE = 255;
        public const int MAX_INT_VALUE = 65535;
        public enum TipoDato
        {
            Char,
            Int,
            Float
        }

        TipoDato tipo;
        string nombre;
        float valor;

        public Variable(TipoDato tipo, string nombre, float valor = 0)
        {
            this.tipo = tipo;
            this.nombre = nombre;
            this.valor = valor;
        }
        public void setValor(float valor, TipoDato maximoTipo, StreamWriter logger)
        {
            if (tipo < maximoTipo)
            {
                throw new Error("Semántico: no se puede asignar un " + maximoTipo + " a un " + tipo, logger);
            }

            setValor(valor);
        }
        public void setValor(float valor)
        {

            if (valorToTipoDato(valor) <= tipo)
            {
                this.valor = valor;
            }
            else
            {
                throw new Error("Semántico: no se puede asignar un " + valorToTipoDato(valor) + " a un " + tipo);
            }
        }

        public static TipoDato valorToTipoDato(float valor)
        {
            if (!float.IsInteger(valor))
            {
                return TipoDato.Float;
            }
            else if (valor <= MAX_CHAR_VALUE)
            {
                return TipoDato.Char;
            }
            else if (valor <= MAX_INT_VALUE)
            {
                return TipoDato.Int;
            }
            else
            {
                return TipoDato.Float;
            }
        }

        public static int getMaxValueByType(TipoDato type)
        {
            if (type == TipoDato.Char) return MAX_CHAR_VALUE;
            else return MAX_INT_VALUE;
        }

        public float getValor()
        {
            return valor;
        }

        public string getNombre()
        {
            return nombre;
        }

        public TipoDato getTipoDato()
        {
            return tipo;
        }
    }
}