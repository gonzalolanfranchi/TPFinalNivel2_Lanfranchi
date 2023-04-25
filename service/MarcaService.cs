using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain;

namespace service
{
    public class MarcaService
    {
        public List<Marca> toList()
        {
            List<Marca> list = new List<Marca>();
            DataAccess datos = new DataAccess();
            try
            {
                datos.setQuery("Select Id, Descripcion From MARCAS");
                datos.executeRead();
                while (datos.Reader.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Reader["Id"];
                    aux.Descripcion = (string)datos.Reader["Descripcion"];
                    list.Add(aux);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.closeConnection();
            }
        }

    }
}
