using System;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace BORRAME
{
    class Program
    {

        static void Main(string[] args)
        {
            string dir = "./recetasalpha.txt";
            string dirRes = "./Archivos/ListaSemanal.pdf";
            Lista listaEntera = CreaLista(dir);
            Lista listaSemana = new Lista();

            EscLista(ref listaSemana, listaEntera);
            GuardaLista(dirRes, listaSemana);
            
            Attachment at = new Attachment(dirRes);

            var fromAddress = new MailAddress("servidorhuebos@gmail.com", "Halfonso");
            var dirAgo = new MailAddress("agrosocas@gmail.com", "Ago");
            var dirRicky = new MailAddress("ricardossmc@gmail.com", "Ricky");
            const string fromPassword = "LosHuebos02089";
            const string subject = "Comidas de esta semanica :D";
            const string body = "";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            var message = new MailMessage(fromAddress, dirAgo)
            {
                Subject = subject,
                Body = body,                
            };
            var message2 = new MailMessage(fromAddress, dirRicky)
            {
                Subject = subject,
                Body = body,                
            };
            message.Attachments.Add(at);
            message2.Attachments.Add(at);
            smtp.Send(message);
            smtp.Send(message2);
            
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
            StreamWriter escritura = new StreamWriter(dir);
            
            escritura.WriteLine("Recetas Guapas de la Semana -" +System.DateTime.Now.ToString("M/d/yyyy") + ": \n\n");
            for(int i = 0; i<l.index;i++)
            {
                escritura.WriteLine(l.recetas[i].Nombre + l.recetas[i].Categoria + " " + l.recetas[i].frec + "\n");
            }
            escritura.Close();
        }
        
        static void EscLista(ref Lista nueva, Lista l)
        {
            Console.WriteLine("EstoyEnEscLista");
            nueva.AddReceta(l.BuscaReceta(2));
            nueva.AddReceta(l.BuscaReceta(1));
            nueva.AddReceta(l.BuscaReceta(1));
            nueva.AddReceta(l.BuscaReceta(0, "P"));
            nueva.AddReceta(l.BuscaReceta(0, "L"));
            nueva.AddReceta(l.BuscaReceta(0, "C"));
            nueva.AddReceta(l.BuscaReceta(0, "C"));
                        
        }


        static Receta LeeReceta(ref StreamReader reader)
        {
            string nombre;
            string proceso;
            Ingrediente[] ingredientes;
            string cat;
            int frec;

            string[] linea;
            do 
            {
                linea = reader.ReadLine().Split(' ');
            } while(linea[0] == "");
            nombre = linea[0];
            frec = int.Parse(linea[1]);
            cat = linea[2].ToUpper();

            reader.ReadLine();
            ingredientes = LeeIngredientes(ref reader);
            proceso = LeeProceso(ref reader);

            Receta receta = new Receta(nombre, cat, frec);

            return receta;
        }
        static Ingrediente[] LeeIngredientes(ref StreamReader reader)
        {
            string[] lineaS = reader.ReadLine().Split(' ');
            Ingrediente[] ingredientes = new Ingrediente[300];
            int i = 0;
            while (lineaS[0] != "" && lineaS[0][0] == '-')
            {
                if (lineaS.Length > 1) ingredientes[i] = new Ingrediente(lineaS[0], int.Parse(lineaS[1]), (lineaS[2]));
                else ingredientes[i] = new Ingrediente(lineaS[0]);
                lineaS = reader.ReadLine().Split(' ');
                i++;
            }
            return ingredientes;
        }
        static string LeeProceso(ref StreamReader reader)
        {
            string res = "";
            char j = ' ';
            do
            {j = (char)reader.Read();}
            while (j!='"'); //se salta las primeras comillas
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
        public string medida;

        public Ingrediente(string _n, int _c, string _m)
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
        public string Categoria;
        public int frec;

        public Receta(string _nombre, Ingrediente[] _ingredientes, string _proceso, string unidad, int _frec)
        {
            Nombre = _nombre;
            ingredientes = _ingredientes;
            Proceso = _proceso;
            Categoria = unidad;
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
            index = 0;
            cuentaIngredientes = 0;

        }
        public Receta BuscaReceta(int i)
        {
            Random rnd = new Random();
            int r = rnd.Next(0, index);
            while(recetas[r].frec != i)          
            {
                r = rnd.Next(0, index);
            }
            return recetas[r];
        }
        public Receta BuscaReceta(int i, string c)
        {
            bool enc = false;
            Random rnd = new Random();
            int r = rnd.Next(0, index);
            while(!enc)            
            {
                if(recetas[r].frec == i && recetas[r].Categoria == c.ToUpper()) enc = true;
                else r = rnd.Next(0, index);
            }
            return recetas[r];
        }
        public void AddReceta(Receta r)
        {
            recetas[index] = r;
            Console.WriteLine(r.Nombre + " " + r.Categoria + " " + r.frec);

            index++;
            //AddIngredientes(r);
        }
        public void AddIngredientes(Receta r)
        {
            for (int i = 0; i<r.ingredientes.Length; i++)
            {
                listaIngredientes[cuentaIngredientes] = r.ingredientes[i];
                cuentaIngredientes++;
            }
        }


    }


}

