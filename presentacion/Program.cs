using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Crea la carpeta de imagenes y descarga una default si no existe alguna de las dos.
            string carpetaImagenes = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + ConfigurationManager.AppSettings["images"];
            if (!(Directory.Exists(carpetaImagenes)) || !(File.Exists(carpetaImagenes + "\\notfound.png")))
            {
                CarpetaManager carpeta = new CarpetaManager();
                Task task = carpeta.carpetaAsync(carpetaImagenes);
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmArticulos());
        }
    }
    public class CarpetaManager
    {
        public async Task carpetaAsync(string carpetaImagenes)
        {            
            string imgUrl = "https://cdn.icon-icons.com/icons2/1462/PNG/512/120nophoto_100007.png";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    if (!(Directory.Exists(carpetaImagenes)))
                        Directory.CreateDirectory(carpetaImagenes);
                    string rutaGuardado = carpetaImagenes + "\\notfound.png";
                    byte[] imagenBytes = await client.GetByteArrayAsync(imgUrl);
                    File.WriteAllBytes(rutaGuardado, imagenBytes);
                }
                catch (Exception)
                {
                    MessageBox.Show("Falla al crear la carpeta de imagenes o descargar de internet.");
                }
            }        
        }
    }
}
