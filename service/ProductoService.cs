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

        public void eliminar(int id)
        {
            DataAccess datos = new DataAccess();
            try
            {
                datos.setQuery("delete From ARTICULOS where id = @id");
                datos.setParameter("id", id);
                datos.executeAction();
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

        public void agregar(Producto nuevo)
        {
            DataAccess datos = new DataAccess();
            try
            {
                datos.setQuery("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");
                datos.setParameter("@codigo", nuevo.Codigo);
                datos.setParameter("@nombre", nuevo.Nombre);
                datos.setParameter("@descripcion", nuevo.Descripcion);
                datos.setParameter("@idMarca", nuevo.Marca.Id);
                datos.setParameter("@idCategoria", nuevo.Categoria.Id);
                datos.setParameter("@imagenUrl", nuevo.ImagenUrl);
                datos.setParameter("@precio", nuevo.Precio);
                datos.executeAction();
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

        public void modificar(Producto prod)
        {
            DataAccess datos = new DataAccess();
            try
            {
                datos.setQuery("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imagenUrl, Precio = @precio where Id = @id");
                datos.setParameter("@codigo", prod.Codigo);
                datos.setParameter("@nombre", prod.Nombre);
                datos.setParameter("@descripcion", prod.Descripcion);
                datos.setParameter("@idMarca", prod.Marca.Id);
                datos.setParameter("@idCategoria", prod.Categoria.Id);
                datos.setParameter("@imagenUrl", prod.ImagenUrl);
                datos.setParameter("@precio", prod.Precio);
                datos.setParameter("Id", prod.Id);
                datos.executeAction();
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

        public List<Producto> filtrar(string campo, string criterio, string filtro)
        {
            List<Producto> list = new List<Producto>();
            DataAccess data = new DataAccess();

            try
            {
                string query = "Select A.Id, Codigo, Nombre, A.Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio, C.Descripcion Categoria, M.Descripcion Marca From ARTICULOS A, CATEGORIAS C, MARCAS M Where IdCategoria = C.Id AND IdMarca = M.Id AND ";

                switch (campo)
                {
                    case "Precio":
                        switch (criterio)
                        {
                            case "Menor que":
                                query += "Precio < " + filtro;
                                break;

                            case "Mayor que":
                                query += "Precio > " + filtro;
                                break;

                            default: //Igual que
                                query += "Precio = " + filtro;
                                break;
                        }

                        break;

                    case "Marca":
                        switch (criterio)
                        {
                            case "Contiene":
                                query += "M.Descripcion like '%" + filtro + "%'";
                                break;

                            case "Termina con":
                                query += "M.Descripcion like '%" + filtro + "'";
                                break;

                            default: //Comienza con
                                query += "M.Descripcion like '" + filtro + "%'";
                                break;
                        }
                        break;

                    case "Categoria":
                        switch (criterio)
                        {
                            case "Contiene":
                                query += "C.Descripcion like '%" + filtro + "%'";
                                break;

                            case "Termina con":
                                query += "C.Descripcion like '%" + filtro + "'";
                                break;

                            default: //Comienza con
                                query += "C.Descripcion like '" + filtro + "%'";
                                break;
                        }
                        break;

                    default: //Nombre
                        switch (criterio)
                        {
                            case "Contiene":
                                query += "Nombre like '%" + filtro + "%'";
                                break;

                            case "Termina con":
                                query += "Nombre like '%" + filtro + "'";
                                break;

                            default: //Comienza con
                                query += "Nombre like '" + filtro + "%'";
                                break;
                        }
                        break;
                }
                data.setQuery(query);
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
