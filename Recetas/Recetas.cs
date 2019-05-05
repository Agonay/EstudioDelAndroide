using System;

namespace Recetas
{
    class Receta
    {
        public string Nombre;
        public Ingrediente[] ingredientes;
        public string Proceso;
        public string Categoria;
        public int frec;

        #region Constructores
        public Receta(string _nombre, Ingrediente[] _ingredientes, string _proceso, string _cat, int _frec)
        {
            Nombre = _nombre;
            ingredientes = _ingredientes;
            Proceso = _proceso;
            Categoria = _cat;
            frec = _frec;
        }
        public Receta(string _nombre, Ingrediente[] _ingredientes, string unidad, int _frec)
        {
            Nombre = _nombre;
            ingredientes = _ingredientes;
            Categoria = unidad;
            frec = _frec;
        }
        public Receta(string _nombre, string unidad, int _frec)
        {
            Nombre = _nombre;
            Categoria = unidad;
            frec = _frec;
        }
        #endregion
    }
    class Ingrediente
    {
        public enum tipoI
        {
            especia, principal, opcional
        }

        public string nombre;
        public int cantidad;
        public string medida;
        public tipoI tipo;

        #region Constructores
        public Ingrediente(string _n, int _c, string _m, string _tipo)
        {
            nombre = _n;
            cantidad = _c;
            medida = _m;
            if (!System.Enum.TryParse<tipoI>(_tipo, out tipo))
            {
                tipo = tipoI.especia;
            }

        }
        public Ingrediente(string _n, int _c, string _m)
        {
            nombre = _n;
            cantidad = _c;
            medida = _m;
            tipo = tipoI.especia;
        }
        public Ingrediente(string _n, string _tipo)
        {
            nombre = _n;
            cantidad = 1;
            medida = "";
            if (!System.Enum.TryParse<tipoI>(_tipo, out tipo))
            {
                tipo = tipoI.especia;
            }
        }
        public Ingrediente(string _n)
        {
            nombre = _n;
            cantidad = 1;
            medida = "";
            tipo = tipoI.especia;

        }
        #endregion
    }
}
