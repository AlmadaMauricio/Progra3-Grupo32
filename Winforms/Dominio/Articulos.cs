using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulos
    {
        public int IdArticulo { get; set; }

        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Marcas Marcas { get; set; }
        public Categoria Categoria { get; set; }
        public float Precio { get; set; }
        public Imagenes imagenes { get; set; }

    }
}
