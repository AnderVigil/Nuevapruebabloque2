using System;
using System.Data;
using System.Windows.Forms;

namespace ACTIVIDADBLOQUE2_otro_intento
{
    public partial class Form1 : Form
    {
        db_conexion objConexion = new db_conexion();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public int posicion = 0;
        String accion = "Nuevo"; // Acción inicial

        public Form1()
        {
            InitializeComponent();
            objConexion = new db_conexion();

            // Verifica la conexión
            if (objConexion.TestConexion())
            {
                obtenerDatos(); // Solo llama a obtenerDatos si la conexión es exitosa
            }
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Estás seguro de que quieres salir?", "Confirmar Salida", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            obtenerDatos();
        }

        private void obtenerDatos()
        {
            ds = objConexion.obtenerDatos();
            dt = ds.Tables["peliculas"];
            dgvPeliculas.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                dt.PrimaryKey = new DataColumn[] { dt.Columns["Idmovie"] };
                posicion = 0; // Inicializa la posición
                mostrarDatos();
                actualizarRegistroLabel(); // Actualiza el label al cargar
            }
            else
            {
                MessageBox.Show("No hay datos para mostrar.");
            }
        }

        private void mostrarDatos()
        {
            if (dt.Rows.Count > 0)
            {
                txtTitulo.Text = dt.Rows[posicion]["Titulo"].ToString();
                txtAutor.Text = dt.Rows[posicion]["Autor"].ToString();
                txtSinopsis.Text = dt.Rows[posicion]["Sinopsis"].ToString();
                txtDuracion.Text = dt.Rows[posicion]["Duracion"].ToString();
                txtClasificacion.Text = dt.Rows[posicion]["Clasificacion"].ToString();
            }
            else
            {
                MessageBox.Show("No hay datos disponibles.");
            }
        }

        private void actualizarRegistroLabel()
        {
            lblRegistro.Text = $"{posicion + 1} de {dt.Rows.Count}"; // Actualiza el label con el formato "x de n"
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            if (posicion > 0)
            {
                posicion = 0; // Ir al primer registro
                mostrarDatos();
                actualizarRegistroLabel();
            }
            else
            {
                MessageBox.Show("Ya estás en el primer registro."); // Mensaje si ya está en el primer registro
            }
        }

        private void habDesControles(Boolean estado)
        {
            txtTitulo.Enabled = estado;
            txtAutor.Enabled = estado;
            txtSinopsis.Enabled = estado;
            txtDuracion.Enabled = estado;
            txtClasificacion.Enabled = estado;
            btnEliminar.Enabled = !estado;
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            if (posicion > 0)
            {
                posicion--; // Ir al registro anterior
                mostrarDatos();
                actualizarRegistroLabel();
            }
            else
            {
                MessageBox.Show("Ya estás en el primer registro.");
            }
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (posicion < dt.Rows.Count - 1)
            {
                posicion++; // Ir al siguiente registro
                mostrarDatos();
                actualizarRegistroLabel();
            }
            else
            {
                MessageBox.Show("Ya estás en el último registro."); // Mensaje cuando ya está en el último registro
            }
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            if (posicion < dt.Rows.Count - 1)
            {
                posicion = dt.Rows.Count - 1; // Ir al último registro
                mostrarDatos();
                actualizarRegistroLabel();
            }
            else
            {
                MessageBox.Show("Ya estás en el último registro."); // Mensaje si ya está en el último registro
            }
        }

        private void limpiarCajas()
        {
            txtTitulo.Text = "";
            txtAutor.Text = "";
            txtSinopsis.Text = "";
            txtDuracion.Text = "";
            txtClasificacion.Text = "";
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (btnNuevo.Text == "Nuevo")
            {
                btnNuevo.Text = "Guardar";
                btnModificar.Text = "Cancelar";
                limpiarCajas();
                accion = "nuevo";
                habDesControles(true);
            }
            else
            {
                // Guardar
                if (objConexion.AgregarPelicula(txtTitulo.Text, txtAutor.Text, txtSinopsis.Text, txtDuracion.Text, txtClasificacion.Text))
                {
                    obtenerDatos();
                    habDesControles(false);
                    btnNuevo.Text = "Nuevo";
                    btnModificar.Text = "Modificar";
                }
                else
                {
                    MessageBox.Show("Error al guardar la película.");
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (btnModificar.Text == "Modificar")
            {
                btnModificar.Text = "Cancelar";
                btnNuevo.Text = "Guardar";
                accion = "modificar";
                habDesControles(true);
            }
            else
            {
                // Cancelar
                mostrarDatos();
                habDesControles(false);
                btnModificar.Text = "Modificar";
                btnNuevo.Text = "Nuevo";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de eliminar: " + txtTitulo.Text + "?", "Eliminando película",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (objConexion.EliminarPelicula(Convert.ToInt32(dt.Rows[posicion]["Idmovie"])))
                {
                    obtenerDatos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar la película.");
                }
            }



        }
        private void filtrarDatos(String buscar)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "Titulo like '%" + buscar + "%' OR Autor like '%" + buscar + "%'";
            dgvPeliculas.DataSource = dv;
        }

        private void txtbuscar_KeyUp(object sender, KeyEventArgs e)
        {
            filtrarDatos(txtbuscar.Text);

        }


    }
}
