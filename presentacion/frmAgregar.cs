using domain;
using service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class frmAgregar : Form
    {
        private Producto producto = null;
        private OpenFileDialog archivo = null;
        public frmAgregar()
        {
            InitializeComponent();
        }

        public frmAgregar(Producto producto, bool modificable)
        {
            InitializeComponent();
            this.producto = producto;
            if (modificable)
            {
                Text = "Modificar producto";
                loadImage(producto.ImagenUrl);
            }
            else
            {
                Text = "Detalle del articulo";
                loadImage(producto.ImagenUrl);
                txtCodigo.Enabled = false;
                txtNombre.Enabled = false;
                txtDescripcion.Enabled = false;
                cboMarca.Enabled = false;
                cboCategoria.Enabled = false;
                txtImagenUrl.Enabled = false;
                txtPrecio.Enabled = false;
                btnAceptar.Visible = false;
                btnAgregarImagen.Visible = false;
            }
        }

        private void frmAgregar_Load(object sender, EventArgs e)
        {
            MarcaService marcaService = new MarcaService();
            CategoriaService categoriaService = new CategoriaService();
            try
            {
                cboMarca.DataSource = marcaService.toList();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaService.toList();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (producto != null)
                {
                    txtCodigo.Text = producto.Codigo;
                    txtNombre.Text = producto.Nombre;
                    txtDescripcion.Text = producto.Descripcion;
                    cboMarca.SelectedValue = producto.Marca.Id;
                    cboCategoria.SelectedValue = producto.Categoria.Id;
                    txtImagenUrl.Text = producto.ImagenUrl;
                    txtPrecio.Text = producto.Precio.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ProductoService service = new ProductoService();
            try
            {
                if (producto == null)
                    producto = new Producto();
                producto.Codigo = txtCodigo.Text;
                producto.Nombre = txtNombre.Text;
                producto.Descripcion = txtDescripcion.Text;
                producto.Marca = (Marca)cboMarca.SelectedItem;
                producto.Categoria = (Categoria)cboCategoria.SelectedItem;
                producto.ImagenUrl = txtImagenUrl.Text;
                loadImage(producto.ImagenUrl);
                producto.Precio = decimal.Parse(txtPrecio.Text);

                if (archivo != null && !(txtImagenUrl.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + ConfigurationManager.AppSettings["images"] + archivo.SafeFileName);
                    producto.ImagenUrl = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + ConfigurationManager.AppSettings["images"] + archivo.SafeFileName;
                }
                if (producto.Id != 0)
                {
                    service.modificar(producto);
                    MessageBox.Show("Modificado exitosamente!");
                }
                else
                {
                    service.agregar(producto);
                    MessageBox.Show("Agregado exitosamente!");
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                loadImage(archivo.FileName);
            }
        }

        private void loadImage(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {
                string sinimagen = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + ConfigurationManager.AppSettings["images"] + "\\notfound.png";
                pbxImagen.Load(sinimagen);
            }
        }
    }
}
