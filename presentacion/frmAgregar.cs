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

        public frmAgregar(Producto producto, bool modificable = false)
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
                txtCodigo.ReadOnly = true;
                txtNombre.ReadOnly = true;
                txtDescripcion.ReadOnly = true;
                cboMarca.Enabled = false;
                cboCategoria.Enabled = false;
                txtImagenUrl.ReadOnly = true;
                txtPrecio.ReadOnly = true;
                btnAceptar.Visible = false;
                btnAgregarImagen.Visible = false;
                btnCancelar.Text = "Cerrar";
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
                else
                {
                    loadImage("");
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
                if (validateProduct())
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
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool validateProduct()
        {
            if (txtNombre.Text == null || txtNombre.Text == "")
            {
                MessageBox.Show("El producto debe tener un nombre.");
                return false;
            }
            if (txtPrecio.Text == null || txtPrecio.Text == "")
            {
                MessageBox.Show("El producto debe tener un precio.");
                return false;
            }
            if (correctPriceFormat(txtPrecio.Text))
            {
                MessageBox.Show("El precio debe ser un formato correcto y ser solo numeros.");
                return false;
            }
            if (txtCodigo.TextLength > 50)
            {
                MessageBox.Show("El codigo es demasiado largo. Maximo 50 caracteres.");
                return false;
            }
            if (txtNombre.TextLength > 50)
            {
                MessageBox.Show("El nombre es demasiado largo. Maximo 50 caracteres.");
                return false;
            }
            if (txtDescripcion.TextLength > 150)
            {
                MessageBox.Show("La descripcion es demasiado larga. Maximo 150 caracteres.");
                return false;
            }
            if (txtImagenUrl.TextLength > 1000)
            {
                MessageBox.Show("La URL de la imagen es demasiado larga. Maximo 1000 caracteres.");
                return false;
            }

            return true;
        }

        private bool correctPriceFormat(string chain) //NO ME SALIO ESTO TODAVIA, REHACER
        {
            chain = chain.Replace('.' , ',');
            int commaCounter = 0;

            foreach (char character in chain)
            {
                if ( char.IsNumber(character) ) 
                {
                }else if ( character == ',' )
                {
                    commaCounter++;
                }
                else
                {
                    return true;
                }

            }   
            if (commaCounter == 0 || commaCounter == 1)
            {
                txtPrecio.Text = chain;
                return false;
            }
            else
            {
            return true;
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
