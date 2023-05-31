using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoria");
            cboCampo.Items.Add("Precio");
            cboCampo.SelectedItem = "Nombre";

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
                dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";
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
            catch (Exception)
            {
                try
                {
                    string sinimagen = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + ConfigurationManager.AppSettings["images"] + "\\notfound.png";
                    pbxImagen.Load(sinimagen);

                }
                catch (Exception ex)
                {
                    string sinimagen = "https://cdn.icon-icons.com/icons2/1462/PNG/512/120nophoto_100007.png";
                    pbxImagen.Load(sinimagen);
                    throw ex;
                }
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

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void eliminar()
        {
            ProductoService service = new ProductoService();
            Producto seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Esta seguro que desea eliminar el producto?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Producto)dgvArticulos.CurrentRow.DataBoundItem;
                    service.eliminar(seleccionado.Id);
                    load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregar alta = new frmAgregar();
            alta.ShowDialog();
            load();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Producto seleccionado;
            seleccionado = (Producto)dgvArticulos.CurrentRow.DataBoundItem;
            frmAgregar modificar = new frmAgregar(seleccionado, true);
            modificar.ShowDialog();
            load();
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            Producto seleccionado;
            seleccionado = (Producto)dgvArticulos.CurrentRow.DataBoundItem;
            frmAgregar detalles = new frmAgregar(seleccionado);
            detalles.ShowDialog();
            load();

        }

        private void filtrar(string filtro)
        {
            List<Producto> filteredList;

            if (filtro != "")
            {
                filteredList = listaProducto.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Marca.Descripcion.ToLower().Contains(filtro.ToLower()) || x.Categoria.Descripcion.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                filteredList = listaProducto;
            }
            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = filteredList;
            hideColumns();
            dgvArticulos.Columns["Precio"].DefaultCellStyle.Format = "0.00";
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            filtrar(txtFiltro.Text);
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string option = cboCampo.SelectedItem.ToString();
            if (option == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Menor que");
                cboCriterio.Items.Add("Igual que");
                cboCriterio.Items.Add("Mayor que");
                cboCriterio.SelectedItem = "Igual que";
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
                cboCriterio.SelectedItem = "Comienza con";
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ProductoService service = new ProductoService();

            try
            {
                if (validateFilter())
                    return;
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvArticulos.DataSource = service.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool validateFilter()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (txtFiltro.Text == "")
                {
                    MessageBox.Show("Por favor, ingrese un numero para filtrar");
                    return true;
                }
                if (!(onlyNumbers(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo numeros para filtrar por precio!");
                    return true;
                }
            }

            

            return false;
        }

        private bool onlyNumbers(string chain)
        {
            foreach (char character in chain)
            {
                if (!(char.IsNumber(character)))
                    return false;
            }
            return true;
        }

        private void btnResetear_Click(object sender, EventArgs e)
        {
            filtrar("");
            txtFiltro.Text = "";
            cboCampo.SelectedItem = "Nombre";
        }

        
    }
}
