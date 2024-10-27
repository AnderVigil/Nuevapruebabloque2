using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ACTIVIDADBLOQUE2_otro_intento
{
    internal class db_conexion
    {
        SqlConnection miConexion = new SqlConnection();
        SqlCommand miComando = new SqlCommand();
        SqlDataAdapter miAdaptador = new SqlDataAdapter();
        DataSet ds = new DataSet();

        public db_conexion()
        {
            miConexion.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_peliculas.mdf;Integrated Security=True";
        }

        public DataSet obtenerDatos()
        {
            try
            {
                ds.Clear();
                miComando.Connection = miConexion;
                miComando.CommandText = "SELECT * FROM peliculas";
                miAdaptador.SelectCommand = miComando;
                miAdaptador.Fill(ds, "peliculas");
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos: " + ex.Message);
                return null;
            }
        }

        public bool TestConexion()
        {
            try
            {
                miConexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
                return false;
            }
            finally
            {
                miConexion.Close();
            }
        }

        public bool AgregarPelicula(string titulo, string autor, string sinopsis, string duracion, string clasificacion)
        {
            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;
                miComando.CommandText = "INSERT INTO peliculas (Titulo, Autor, Sinopsis, Duracion, Clasificacion) VALUES (@Titulo, @Autor, @Sinopsis, @Duracion, @Clasificacion)";

                miComando.Parameters.Clear();
                miComando.Parameters.AddWithValue("@Titulo", titulo);
                miComando.Parameters.AddWithValue("@Autor", autor);
                miComando.Parameters.AddWithValue("@Sinopsis", sinopsis);
                miComando.Parameters.AddWithValue("@Duracion", duracion);
                miComando.Parameters.AddWithValue("@Clasificacion", clasificacion);

                int filasAfectadas = miComando.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la película: " + ex.Message);
                return false;
            }
            finally
            {
                miConexion.Close();
            }
        }

        public bool EliminarPelicula(int id)
        {
            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;
                miComando.CommandText = "DELETE FROM peliculas WHERE Idmovie = @Idmovie";
                miComando.Parameters.Clear();
                miComando.Parameters.AddWithValue("@Idmovie", id);
                int filasAfectadas = miComando.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar película: " + ex.Message);
                return false;
            }
            finally
            {
                miConexion.Close();
            }
        }

        public bool ModificarPelicula(int id, string titulo, string autor, string sinopsis, string duracion, string clasificacion)
        {
            try
            {
                miConexion.Open();
                miComando.Connection = miConexion;
                miComando.CommandText = "UPDATE peliculas SET Titulo = @Titulo, Autor = @Autor, Sinopsis = @Sinopsis, Duracion = @Duracion, Clasificacion = @Clasificacion WHERE Idmovie = @Idmovie";
                miComando.Parameters.Clear();
                miComando.Parameters.AddWithValue("@Titulo", titulo);
                miComando.Parameters.AddWithValue("@Autor", autor);
                miComando.Parameters.AddWithValue("@Sinopsis", sinopsis);
                miComando.Parameters.AddWithValue("@Duracion", duracion);
                miComando.Parameters.AddWithValue("@Clasificacion", clasificacion);
                miComando.Parameters.AddWithValue("@Idmovie", id);

                int filasAfectadas = miComando.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar película: " + ex.Message);
                return false;
            }
            finally
            {
                miConexion.Close();
            }
        }
    }
}
