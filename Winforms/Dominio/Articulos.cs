using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dominio
{
    public class Articulos
    {
        public int IdArticulo { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        public int IdMarca { get; set; }
        public Marcas Marcas { get; set; }
        public int IdCategoria {get; set; }
        public Categoria Categoria { get; set; }
        public float Precio { get; set; }
        public Imagenes imagenes { get; set; }
        /// 





        [DisplayName("Marca")]
        public Marcas NombreMarca { get; set; }

        [DisplayName("Categoria")]
        public Categoria TipoCategoria { set; get; }

        public List<Imagenes> UrlImagen { set; get; }


    }
}
