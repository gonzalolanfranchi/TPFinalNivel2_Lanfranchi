using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using domain;
using service;

namespace presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Producto> listaProducto;
        public frmArticulos()
        {
            InitializeComponent();
        }

        private void frmArticulos_Load(object sender, EventArgs e)
        {
            load();
            
        }

        private void load()
        {
            ProductoService service = new ProductoService();
            try
            {
                listaProducto = service.toList();
                dgvArticulos.DataSource = listaProducto;
                hideColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void hideColumns()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;

        }
    }
}
