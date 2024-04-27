using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace TP_Articulos
{
    public partial class ListaDeArticulo : Form
    {
        private List<Articulos> listaArticulos;
        public ListaDeArticulo()
        {
            InitializeComponent();
        }

        private int indice = 0;
        private List<Articulos> articulos;
        private Articulos seleccion;
       

        private void ListarArticulos()
        {
            ArticulosNegocio art = new ArticulosNegocio();
            try
            {
                articulos = art.listar();
                dgvArticulos.DataSource = articulos;
                dgvArticulos.Columns["id"].Visible = false;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            listaArticulos = negocio.listar();
            dgvArticulos.DataSource = negocio.listar();
            ocultarColumnas();
            cargarImagen(listaArticulos[0].imagenes.ImagenUrl);
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.imagenes.ImagenUrl);
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["imagenes"].Visible = false;
            dgvArticulos.Columns["IdArticulo"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulos.Load("https://i0.wp.com/sunrisedaycamp.org/wp-content/uploads/2020/10/placeholder.png?ssl=1");
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listaArticulos.FindAll(x => x.Codigo.ToUpper().Contains(filtro.ToUpper()) || x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Marcas.DescripcionMarca.ToUpper().Contains(filtro.ToUpper()) || x.Categoria.DescripcionCategoria.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaArticulos;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
        }

        private void btnEliminarArticulo_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            Articulos seleccionado;
            try
            {
                if (dgvArticulos.CurrentRow != null)
                    seleccionado = (Articulos)dgvArticulos.CurrentRow.DataBoundItem;
                else
                {
                    MessageBox.Show("No ha seleccionado ningun articulo");
                    return;
                }
                DialogResult respuesta = MessageBox.Show("Usted quiere eliminar este articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    negocio.eliminarArticulo(seleccionado.IdArticulo);
                    ListarArticulos();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnDetalleArticulo_Click_1(object sender, EventArgs e)
        {
            ArticulosNegocio art = new ArticulosNegocio();

            try
            {
                using (DetalleArticulo ventanaDarticulo = new DetalleArticulo(seleccion))
                    ventanaDarticulo.ShowDialog();
                ListarArticulos();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
