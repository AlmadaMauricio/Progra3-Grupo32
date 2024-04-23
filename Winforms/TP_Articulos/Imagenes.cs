using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_Articulos
{
    public class Imagenes
    {
        public int IdImagen { get; set; }
        public int IdArticulo { get; set; }
        public string ImagenUrl { get; set; }
        public override string ToString()
        {
            return ImagenUrl;
        }
    }
}
