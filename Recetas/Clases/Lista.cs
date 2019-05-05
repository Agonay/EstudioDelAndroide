using System;
using Recetas;

namespace HalfonsoComida
{
    class Lista
    {
        #region  Atributos y constructor
        public Receta[] recetas;
        public Ingrediente[] listaIngredientes;
        public int index;
        public int cuentaIngredientes;

        public Lista()
        {
            recetas = new Receta[64];
            listaIngredientes = new Ingrediente[200];
            index = 0;
            cuentaIngredientes = 0;
        }
        #endregion
       
        #region Busca Receta - Métodos
        public bool BuscaReceta(Lista l, Receta r)
        {
            for(int j = 0; j < l.index; j++)
            {
                if(l.recetas[j].Nombre == r.Nombre) return true;
            }
                
            return false;
        }
        public Receta BuscaReceta(int i, Lista l)
        {
            Random rnd = new Random();
            bool cont = true;
            int r = rnd.Next(0, index);
            while( cont )
            {

                if (recetas[r].frec == i && !BuscaReceta(l, recetas[r]))
                    cont = false;
                else r = rnd.Next(0, index);
            }
            return recetas[r];
        }
        public Receta BuscaReceta(int i, string c, Lista l)
        {
            bool enc = false;
            Random rnd = new Random();
            int r = rnd.Next(0, index);
            while(!enc)         
            {

                if(recetas[r].frec == i && recetas[r].Categoria == c.ToUpper() && !BuscaReceta(l, recetas[r])) enc = true;
                else r = rnd.Next(0, index);
            }
            return recetas[r];
        }
        #endregion

        public void AddReceta(Receta r)
        {
            if(index>recetas.Length-1)
            {
                System.Array.Resize(ref recetas, index+10);
            }
            recetas[index] = r;
            index++;
            AddIngredientes(r);
        }
        public void AddIngredientes(Receta r)
        {
            bool enc;
            foreach (Ingrediente v in r.ingredientes)
            {
                v.nombre = Normaliza(v.nombre);
                enc = false;
                int c = 0;
                while (c < cuentaIngredientes && !CompIngredientes(v, listaIngredientes[c])) c++;
                if (c < cuentaIngredientes) enc = CompIngredientes(v, listaIngredientes[c]);
                if (enc)
                {
                    listaIngredientes[c].cantidad += v.cantidad;
                }
                else
                {
                    listaIngredientes[cuentaIngredientes] = v;
                    cuentaIngredientes++;
                }
            }
        }
        private static bool CompIngredientes(Ingrediente v, Ingrediente i)
        {
            string n1 = v.nombre.ToLower();
            string n2 = i.nombre.ToLower();

            return n1 == n2;
        }
        private static string Normaliza(string n1)
        {
            if(n1[1]=='(')
            {
                n1 = n1.Remove(1,1);
                n1 = n1.Remove(n1.Length-1,1);
            } 
            if (n1[n1.Length-1] == 's' && !n1.EndsWith("res")) n1 = n1.Remove(n1.Length - 1, 1); //quita la 's' pa comparar mejor
            Console.WriteLine(n1);
            
            return n1;
        }
    }
}

