using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain;

namespace service
{
    public class CategoriaService
    {
        public List<Categoria> toList()
        {
            List<Categoria> list = new List<Categoria>();
            DataAccess datos = new DataAccess();
            try
            {
                datos.setQuery("Select Id, Descripcion From CATEGORIAS");
                datos.executeRead();
                while (datos.Reader.Read())
                {
                    Categoria aux = new Categoria();
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
