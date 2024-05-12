using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using static System.Net.Mime.MediaTypeNames;

namespace Negocio
{
    public class ArticulosNegocio
    {
        public List<Articulos> listar()
        {
            List<Articulos> lista = new List<Articulos>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;


            try
            {
                conexion.ConnectionString = "server=.\\SQLLab3; database=CATALOGO_P3_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "SELECT A.Id ID,A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.Precio, I.ImagenUrl Imagen, A.IdCategoria, A.IdMarca from ARTICULOS A, MARCAS M, CATEGORIAS C, IMAGENES I WHERE A.IdMarca = M.Id AND A.IdCategoria = C.Id AND A.Id = I.IdArticulo";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.IdArticulo = (int)lector["ID"];

                    if (!(lector["Codigo"] is DBNull))
                        aux.Codigo = (string)lector["Codigo"];

                    if (!(lector["Nombre"] is DBNull))
                        aux.Nombre = (string)lector["Nombre"];

                    if (!(lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)lector["Descripcion"];

                    aux.IdMarca = (int)lector["IdMarca"];
                    aux.Marcas = new Marcas();
                    if (!(lector["Marca"] is DBNull))
                        aux.Marcas.DescripcionMarca = (string)lector["Marca"];

                    aux.IdCategoria = (int)lector["IdCategoria"];
                    aux.Categoria = new Categoria();
                    if (!(lector["Categoria"] is DBNull))
                        aux.Categoria.DescripcionCategoria = (string)lector["Categoria"];

                    aux.imagenes = new Imagenes();
                    aux.imagenes.ImagenUrl = (string)lector["Imagen"];

                    if (!(lector["Precio"] is DBNull))
                        aux.Precio = (float)(decimal)lector["Precio"];

                    List<Imagenes> imagenes;
                    imagenes = obtenerImagenes(aux.IdArticulo);
                    if (imagenes.Count > 0)
                        aux.UrlImagen = imagenes;

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<Imagenes> obtenerImagenes(int id)
        {
            List<Imagenes> listadoImagenes = new List<Imagenes>();
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("Select I.Id, I.ImagenUrl, A.Id Articulo from IMAGENES I, ARTICULOS A Where A.Id = I.IdArticulo");
            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    int articulo = (int)datos.Lector["Articulo"];
                    if (articulo == id)
                    {
                        Imagenes imagen = new Imagenes();
                        imagen.IdImagen = (int)datos.Lector["Id"];
                        imagen.IdArticulo = articulo;
                        imagen.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                        listadoImagenes.Add(imagen);
                    }
                }
                return listadoImagenes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //A la query tambien falta agregar el parametro de url. Pendiente
                datos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio) VALUES (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Precio)");
                datos.setearParametros("@Codigo", nuevo.Codigo);
                datos.setearParametros("@Nombre", nuevo.Nombre);
                datos.setearParametros("@Descripcion", nuevo.Descripcion);
                datos.setearParametros("@IdMarca", nuevo.Marcas.IdMarca);
                datos.setearParametros("@IdCategoria", nuevo.Categoria.IdCategoria);
                datos.setearParametros("@Precio", nuevo.Precio);

                // Falta insertar el IdArticulo para poder setear el UrlImagen, no se logro traer el IdArticulo del registro que se esta cargando
                //datos.setearConsulta("INSERT INTO IMAGENES (ImagenUrl) VALUES (@ImagenUrl)");
                //datos.SetearParametros("@ImagenUrl", nueva.ImagenUrl);
                AgregarImagenes(nuevo.UrlImagen);
                datos.ejecutarAccion();
 
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        private int obtenerUltimoIdArticulos()
        {
            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("select Id from Articulos");
            int id = 0;
            try
            {
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    id = (int)datos.Lector["Id"];
                }
                return id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        private void AgregarImagenes(List<Imagenes> imagenes)
        {
            int id = obtenerUltimoIdArticulos();
            foreach (Imagenes aux in imagenes)
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("Insert into Imagenes (IdArticulo,ImagenUrl) values(@idArticulo,@UrlImagen)");
                datos.setearParametros("@idArticulo", id);
                datos.setearParametros("@UrlImagen", aux.ImagenUrl);
                try
                {
                    datos.ejecutarAccion();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }

        }
        public void eliminarArticulo(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from ARTICULOS where id = @id");
                datos.SetearParametros("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Articulos> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id ID,A.Codigo, A.Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.Precio, I.ImagenUrl Imagen, A.IdCategoria, A.IdMarca from ARTICULOS A, MARCAS M, CATEGORIAS C, IMAGENES I WHERE A.IdMarca = M.Id AND A.IdCategoria = C.Id AND A.Id = I.IdArticulo and ";

                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "A.Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "A.Precio < " + filtro;
                            break;
                        default:
                            consulta += "A.Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Codigo")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Nombre like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "A.Nombre like '%" + filtro + "%'";
                            break;
                        default:
                            consulta += "A.Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Descripcion")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Marcas")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                        default:
                            consulta += "M.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                        default:
                            consulta += "C.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.IdArticulo = (int)datos.Lector["ID"];

                    if (!(datos.Lector["Codigo"] is DBNull))
                        aux.Codigo = (string)datos.Lector["Codigo"];

                    if (!(datos.Lector["Nombre"] is DBNull))
                        aux.Nombre = (string)datos.Lector["Nombre"];

                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];

                    aux.IdMarca = (int)datos.Lector["IdMarca"];
                    aux.Marcas = new Marcas();
                    if (!(datos.Lector["Marca"] is DBNull))
                        aux.Marcas.DescripcionMarca = (string)datos.Lector["Marca"];

                    aux.IdCategoria = (int)datos.Lector["IdCategoria"];
                    aux.Categoria = new Categoria();
                    if (!(datos.Lector["Categoria"] is DBNull))
                        aux.Categoria.DescripcionCategoria = (string)datos.Lector["Categoria"];

                    aux.imagenes = new Imagenes();
                    aux.imagenes.ImagenUrl = (string)datos.Lector["Imagen"];

                    if (!(datos.Lector["Precio"] is DBNull))
                        aux.Precio = (float)(decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void modificar(Articulos articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, Precio = @Precio Where Id = @id");
                datos.setearParametros("@Codigo", articulo.Codigo);
                datos.setearParametros("@Nombre", articulo.Nombre);
                datos.setearParametros("@Descripcion", articulo.Descripcion);
                datos.setearParametros("@IdMarca", articulo.Marcas.IdMarca);
                datos.setearParametros("@IdCategoria", articulo.Categoria.IdCategoria);
                datos.setearParametros("@Precio", articulo.Precio);
                datos.setearParametros("@id", articulo.IdArticulo);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void InsertarImagenes(string url, int idArt)
        {

            AccesoDatos datos = new AccesoDatos();
            datos.setearConsulta("Insert into Imagenes (IdArticulo,ImagenUrl) values(@idarticulo,@UrlImagen)");
            datos.setearParametros("@idarticulo", idArt);
            datos.setearParametros("@UrlImagen", url);
            try
            {
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
