using System.Net.Mail;
using System.Net;
using System.IO;
using System.Linq;
using Recetas;

namespace HalfonsoComida
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirAlpha = "./recetasalpha.txt";
            string dirRec = "./Archivos/ListaSemanal.txt";
            string dirIng = "./Archivos/ListaIngredientes.txt";
            Lista listaEntera = CreaLista(dirAlpha);
            Lista listaSemana = new Lista();

            EscogeLista(ref listaSemana, listaEntera);
            GuardaListaSemanal(dirRec, listaSemana);
            GuardaListaIngredientes(dirIng, listaSemana);
            //EnviaCorreo("./Archivos");
        }

        private static void EnviaCorreo(string dirRes)
        {
            Attachment at = new Attachment(dirRes+"/ListaSemanal.txt");
            Attachment at2 = new Attachment(dirRes+"/ListaIngredientes.txt");

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
            message.Attachments.Add(at2);
            message2.Attachments.Add(at);
            message2.Attachments.Add(at2);
            smtp.Send(message);
            smtp.Send(message2);
        }

        static Lista CreaLista(string dir)
        {
            StreamReader reader = new StreamReader(dir, System.Text.Encoding.UTF8);
            Lista lista = new Lista();
            while (!reader.EndOfStream)
            {
                lista.AddReceta(LeeReceta(ref reader));
            }
            reader.Close();
            return lista;
        }
        static private string CorregirNombre(string n)
        {
            string res = "";
            char sum;
            foreach (char c in n)
            {
                if(c=='_') sum = ' ';
                else sum = c;
                res+= sum;
            }
            return res;
        }
        static void GuardaListaSemanal(string dir, Lista l)
        {
            StreamWriter escritura = new StreamWriter(dir);
            
            escritura.WriteLine("Recetas Guapas de la Semana - " +System.DateTime.Now.ToString("M/d/yyyy") + ": \n\n");
            foreach(Receta r in l.recetas)
            {
                if(r !=null)
                {
                    escritura.WriteLine(CorregirNombre(r.Nombre.Normalize())+ " " + r.Categoria + " " + r.frec);
                }
            }

            escritura.Close();
        }

        static void GuardaListaIngredientes(string dir, Lista l)
        {
            StreamWriter write = new StreamWriter(dir);

            write.WriteLine("Ingredientes:");
            write.WriteLine("\n Principales:");

            foreach (var i in from Ingrediente i in l.listaIngredientes where i != null && i.tipo ==Ingrediente.tipoI.principal select i)
            {
                write.WriteLine("\t {0} - {1} {2}", i.nombre, i.cantidad, i.medida.ToUpper());
            }
            write.WriteLine("\n Opcionales:");

            foreach (var i in from Ingrediente i in l.listaIngredientes where i != null && i.tipo ==Ingrediente.tipoI.opcional select i)
            {
                write.WriteLine("\t {0} - {1} {2}", i.nombre, i.cantidad, i.medida.ToUpper());
            }
            write.WriteLine("\n Especias:");

            foreach (var i in from Ingrediente i in l.listaIngredientes where i != null && i.tipo ==Ingrediente.tipoI.especia select i)
            {
                write.WriteLine("\t {0} - {1} {2}", i.nombre, i.cantidad, i.medida.ToUpper());
            }
        write.Close();
        }
        
        static void EscogeLista(ref Lista nueva, Lista l)
        {
            nueva.AddReceta(l.BuscaReceta(2, nueva));
            nueva.AddReceta(l.BuscaReceta(1, nueva));
            nueva.AddReceta(l.BuscaReceta(1, nueva));
            nueva.AddReceta(l.BuscaReceta(0, "P", nueva));
            nueva.AddReceta(l.BuscaReceta(0, "L", nueva));
            nueva.AddReceta(l.BuscaReceta(0, "C", nueva));
            nueva.AddReceta(l.BuscaReceta(0, "C", nueva));
        }


        static Receta LeeReceta(ref StreamReader reader)
        {
            string nombre;
            string proceso;
            Ingrediente[] ingredientes;
            string unidad;
            int frec;

            string[] linea;
            do 
            {
                linea = reader.ReadLine().Split(' ');
            } while(linea[0] == "");
            nombre = linea[0];
            frec = int.Parse(linea[1]);
            unidad = linea[2].ToUpper();

            reader.ReadLine();
            ingredientes = LeeIngredientes(ref reader);
            proceso = LeeProceso(ref reader);

            Receta receta = new Receta(nombre, ingredientes, unidad, frec);
            return receta;
        }
        static Ingrediente[] LeeIngredientes(ref StreamReader reader)
        {
            int i = 0;
            string[] lineaS = reader.ReadLine().Split(' ');
            Ingrediente[] ingredientes = new Ingrediente[1];

            while (lineaS[0] != "" && lineaS[0][0] == '-')
            {
                //hace más grande el vector si es necesario --- podemos acceder a ingredientes.Length con la medida exacta
                if(i >= ingredientes.Length)
                {
                    /*Ingrediente[] aux = new Ingrediente[i+1];
                    for(int j = 0; j<ingredientes.Length;j++) aux[j] = ingredientes[j];
                    ingredientes = aux;*/
                    System.Array.Resize(ref ingredientes, i+1);
                }
                
                if (lineaS.Length > 2)
                {
                    if(lineaS.Length == 3) ingredientes[i] = new Ingrediente(lineaS[0], int.Parse(lineaS[1]), (lineaS[2]));
                    else ingredientes[i] = new Ingrediente(lineaS[0], int.Parse(lineaS[1]), lineaS[2], lineaS[3]);
                }
                else if (lineaS.Length == 2)
                {
                    ingredientes[i] = new Ingrediente(lineaS[0], lineaS[1]);
                }
                else 
                    ingredientes[i] = new Ingrediente(lineaS[0]);

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
}

