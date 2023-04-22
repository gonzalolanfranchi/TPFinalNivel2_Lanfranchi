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
                loadImage(listaProducto[0].ImagenUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void loadImage(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception ex)
            {
                string sinimagen = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + ConfigurationManager.AppSettings["images"] + "\\notfound.png";
                pbxImagen.Load(sinimagen);
            }
        }

        private void hideColumns()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;

        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Producto seleccionado = (Producto)dgvArticulos.CurrentRow.DataBoundItem;
                loadImage(seleccionado.ImagenUrl);
            }
        }
    }
}
