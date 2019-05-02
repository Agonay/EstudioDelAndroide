using System;
using System.IO;

namespace Recetas
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "./Archivos/Recetas.recipe";
            string dirRes = "./Archivos/ListaSemanal";
            Lista listaSemana = CreaLista(dir);
            GuardaLista(dirRes, listaSemana);
        }


        static Lista CreaLista(string dir)
        {
            StreamReader reader = new StreamReader(dir);
            Lista lista = new Lista();
            while (!reader.EndOfStream)
            {
                lista.AddReceta(LeeReceta(ref reader));
            }
            reader.Close();
            return lista;
        }
        static void GuardaLista(string dir, Lista l)
        {
            StreamWriter streamWriter = new StreamWriter(dir);
            foreach (Receta r in l.recetas)
            {
                streamWriter.WriteLine(r.Nombre + r.Categoria + "\n");
            }
            streamWriter.Close();
        }


        static Receta LeeReceta(ref StreamReader reader)
        {
            string nombre;
            string proceso;
            Ingrediente[] ingredientes;
            char cat;
            Frec frec;

            string[] linea;
            linea = reader.ReadLine().Split(' ');
            nombre = linea[0];
            frec = (Frec)Enum.Parse(typeof(Frec), linea[1]);
            cat = char.Parse(linea[2].ToUpper());

            ingredientes = LeeIngredientes(ref reader);
            proceso = LeeProceso(ref reader);

            Receta receta = new Receta(nombre, cat, frec);

            return receta;
        }
        static Ingrediente[] LeeIngredientes(ref StreamReader reader)
        {
            string[] lineaS = reader.ReadLine().Split(' ');
            Ingrediente[] ingredientes = new Ingrediente[20];
            int i = 0;
            while (lineaS[0][0] == '-')
            {
                if (lineaS.Length >= 0) ingredientes[i] = new Ingrediente(lineaS[0], int.Parse(lineaS[1]), char.Parse(lineaS[3]));
                else ingredientes[i] = new Ingrediente(lineaS[0]);
                i++;
            }
            return ingredientes;
        }
        static string LeeProceso(ref StreamReader reader)
        {
            string res = "";
            reader.Read(); //se salta las primeras comillas
            char key = ' ';
            do
            {
                key = (char)reader.Read();
                res += key;

            } while (key != '"');
            return res;
        }


    }
    class Ingrediente
    {
        public string nombre;
        public int cantidad;
        public char medida;

        public Ingrediente(string _n, int _c, char _m)
        {
            nombre = _n;
            cantidad = _c;
            medida = _m;
        }

        public Ingrediente(string _n)
        {
            nombre = _n;
        }

    }

    class Receta
    {
        public string Nombre;
        public Ingrediente[] ingredientes;
        public string Proceso;
        public char Categoria;
        public Frec frec;

        public Receta(string _nombre, Ingrediente[] _ingredientes, string _proceso, char cat, Frec _frec)
        {
            Nombre = _nombre;
            ingredientes = _ingredientes;
            Proceso = _proceso;
            Categoria = cat;
            frec = _frec;
        }
        public Receta(string _nombre, Ingrediente[] _ingredientes, char _cat, Frec _frec)
        {
            Nombre = _nombre;
            ingredientes = _ingredientes;
            Categoria = _cat;
            frec = _frec;
        }
        public Receta(string _nombre, char _cat, Frec _frec)
        {
            Nombre = _nombre;
            Categoria = _cat;
            frec = _frec;
        }
    }
    public enum Frec
    {
        Mucho, Normal, Poco
    }
    class Lista
    {
        public Receta[] recetas;
        public Ingrediente[] listaIngredientes;
        public int index;
        public int cuentaIngredientes;

        public Lista()
        {
            recetas = new Receta[64];
            listaIngredientes = new Ingrediente[100];
        }
        public void AddReceta(Receta r)
        {
            recetas[index] = r;
            index++;
            AddIngredientes(r);
        }
        public void AddIngredientes(Receta r)
        {
            foreach (Ingrediente i in r.ingredientes)
            {
                listaIngredientes[cuentaIngredientes] = i;
                cuentaIngredientes++;
            }
        }

    }


}
