using domain;
using service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class frmAgregar : Form
    {
        private Producto producto = null;
        public frmAgregar()
        {
            InitializeComponent();
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

                //if (producto != null)
                //{
                //    txtCodigo.Text = producto.Codigo;
                //    txtNombre.Text = producto.Nombre;
                //    txtDescripcion.Text = producto.Descripcion;
                //    cboMarca.SelectedValue = producto.Marca.Id;
                //    cboCategoria.SelectedValue = producto.Categoria.Id;
                //    txtImagenUrl.Text = producto.ImagenUrl;
                //    txtPrecio.Text = producto.Precio.ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
