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
using static System.Net.Mime.MediaTypeNames;

namespace TP_Articulos
{
    public partial class frmAltaArticulo : Form
    {
        Articulos articuloNuevo;
        List<Imagenes> imagen = new List<Imagenes>();
        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        public frmAltaArticulo(Articulos articulo)
        {
            InitializeComponent();
            this.articuloNuevo = articulo;
            Text = "Modificar Articulo";
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio aux = new ArticulosNegocio();
            decimal precio;

            try
            {
                if (!ValidarDatos())
                {
                    articuloNuevo = new Articulos();
                    articuloNuevo.Nombre = txtNombre.Text;
                    articuloNuevo.Codigo = txtCodigo.Text;
                    if (!string.IsNullOrEmpty(txbPrecio.Text))
                    {
                        if (decimal.TryParse(txbPrecio.Text, out precio))
                        {
                            Console.WriteLine("El precio es: " + precio);
                        }
                        else
                        {
                            Console.WriteLine("El valor ingresado no es válido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Por favor ingrese un precio.");
                    }
                    articuloNuevo.Precio = float.Parse(txbPrecio.Text);
                    articuloNuevo.Descripcion = txtDescripcion.Text;
                    articuloNuevo.Marcas = (Marcas)cbxMarca.SelectedItem;
                    articuloNuevo.Categoria = (Categoria)cbxCategoria.SelectedItem;
                    articuloNuevo.UrlImagen = imagen;
                    aux.agregar(articuloNuevo);
                    MessageBox.Show("Agregado exitosamente");
                    Close();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            CatgoriaNegocio categoriaNegocio = new CatgoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();
            try
            {
                cbxCategoria.DataSource = categoriaNegocio.listar();
                cbxMarca.DataSource = marcaNegocio.listar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txbImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txbImagen.Text);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxImagen.Load("https://i0.wp.com/sunrisedaycamp.org/wp-content/uploads/2020/10/placeholder.png?ssl=1");
            }
        }

        private bool ValidarDatos()
        {
            if (cbxCategoria.SelectedIndex < 0)

                return true;


            if (cbxMarca.SelectedIndex < 0)

                return true;

            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text))
                return true;
            if (string.IsNullOrEmpty(txtDescripcion.Text) || string.IsNullOrEmpty(txtDescripcion.Text))
                return true;
            if (string.IsNullOrEmpty(txbPrecio.Text))
            {
                return true;
            }
            if (!VerificarNumeros())
            {
                return true;
            }

            return false;

        }

        private bool VerificarNumeros()
        {
            foreach (char c in txbPrecio.Text)
            {
                if (!(char.IsNumber(c)))
                    return false;
            }
            return true;
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            Imagenes imagenes = new Imagenes();
            try
            {
                imagenes.ImagenUrl = txbImagen.Text;
                if (!(string.IsNullOrEmpty(txbImagen.Text)))
                {
                    imagen.Add(imagenes);
                    txbImagen.Text = string.Empty;
                    MessageBox.Show("Imagen agregada al articulo");
                }
                else
                {
                    MessageBox.Show("Por favor complete con una URL y luego presione el boton");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
