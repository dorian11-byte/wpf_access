using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace AlumnosDB
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //agregamos la conexion
        DataTable alumnosBD; //agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            con = new OleDbConnection();
            //conectamos la base de datos
            con.ConnectionString = "Provider=Microsoft.jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\AlumnosBD.mdb";
            MostrarDatos();

        }

        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from alumnosData";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            alumnosBD = new DataTable();
            da.Fill(alumnosBD);
            gvDatos.ItemsSource = alumnosBD.AsDataView();
            if(alumnosBD.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
           
        }

        private void LimpiarTodo()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            cbGenero.SelectedIndex = 0;
            txtTelefono.Text = "";
            txtDireccion.Text = "";
            btnNuevo.Content = "Nuevo";
            txtId.IsEnabled = true;
        }
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarTodo();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "delete from alumnosData where Id=" + row["Id"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Alumno eliminado correctamente ...");
                LimpiarTodo();
                
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un alumno...");
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtId.Text = row["Id"].ToString();
                txtNombre.Text = row["NOMBRE"].ToString();
                cbGenero.Text = row["GENERO"].ToString();
                txtTelefono.Text = row["TELEFONO"].ToString();
                txtDireccion.Text = row["DIRECCION"].ToString();
                txtId.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar un alumno...");
            }
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            if (txtId.Text!="")
            {
                if (cbGenero.Text!="Selecciona un genero")
                {
                    cmd.CommandText = "insert into alumnosData(ID,NOMBRE,GENERO,TELEFONO,DIRECCION)" +
                        "Values (" + txtId.Text + ", '" + txtNombre.Text + ", '" + cbGenero.Text + ", '" + txtTelefono.Text + ", '" + txtDireccion.Text + "')";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Alumno Registrado correctamente...");
                    LimpiarTodo();
                }
                else
                {
                    MessageBox.Show("No se selecciono el genero...");
                }
            }
            else
            {
                cmd.CommandText = "update alumnosData set NOMBRE='" + txtNombre.Text + "',GENERO='" + cbGenero.Text + "',TELEFONO=" + txtTelefono.Text + "',DIRECCION='" + txtDireccion.Text + "'Where Id='" + txtId.Text;
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Datos del alumno actualizados...");
                LimpiarTodo();
            }
        }

        private void TxtId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
