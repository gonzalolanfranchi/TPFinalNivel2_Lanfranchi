using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using domain;

namespace service
{
    public class ProductoService
    {
        public List<Producto> toList()
        {
            List<Producto> list = new List<Producto>();
            DataAccess data = new DataAccess();

            try
            {
                data.setQuery("Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion Categoria, M.Descripcion Marca From ARTICULOS A, CATEGORIAS C, MARCAS M Where IdCategoria = C.Id AND IdMarca = M.Id\r\n");
                data.executeRead();
                while (data.Reader.Read())
                {
                    Producto aux = new Producto();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Codigo = (string)data.Reader["Codigo"];
                    aux.Nombre = (string)data.Reader["Nombre"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)data.Reader["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Reader["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)data.Reader["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Reader["Categoria"];
                    aux.ImagenUrl = (string)data.Reader["ImagenUrl"];
                    aux.Precio = (decimal)data.Reader["Precio"];

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
                data.closeConnection();
            }



        }
    }
}
