using System;
using System.Collections.Generic;
using MySqlConnector;
using SSE.DAL.Conexion;
using SSE.Entidades.Modelos;

namespace SSE.DAL.Repositorios
{
    public class ModalidadTitulacionRepository
    {
        // CREATE (Insertar)
        public bool Insertar(ModalidadTitulacion modalidad)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "INSERT INTO modalidades_titulacion (nombre_modalidad, activo) VALUES (@nombre, @activo)";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", modalidad.NombreModalidad);
                    cmd.Parameters.AddWithValue("@activo", modalidad.Activo);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // READ (Leer Todas) Para el panel de administración
        public List<ModalidadTitulacion> ObtenerTodas()
        {
            List<ModalidadTitulacion> lista = new List<ModalidadTitulacion>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT id_modalidad, nombre_modalidad, activo FROM modalidades_titulacion";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ModalidadTitulacion
                            {
                                IdModalidad = Convert.ToInt32(reader["id_modalidad"]),
                                NombreModalidad = reader["nombre_modalidad"].ToString(),
                                Activo = Convert.ToBoolean(reader["activo"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // READ (Leer solo Activas)  Para llenar los ComboBox de la UI
        public List<ModalidadTitulacion> ObtenerSoloActivas()
        {
            List<ModalidadTitulacion> lista = new List<ModalidadTitulacion>();
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT id_modalidad, nombre_modalidad, activo FROM modalidades_titulacion WHERE activo = 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ModalidadTitulacion
                            {
                                IdModalidad = Convert.ToInt32(reader["id_modalidad"]),
                                NombreModalidad = reader["nombre_modalidad"].ToString(),
                                Activo = Convert.ToBoolean(reader["activo"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        // UPDATE (Actualizar nombre o estado activo/inactivo)
        public bool Actualizar(ModalidadTitulacion modalidad)
        {
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "UPDATE modalidades_titulacion SET nombre_modalidad = @nombre, activo = @activo WHERE id_modalidad = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", modalidad.NombreModalidad);
                    cmd.Parameters.AddWithValue("@activo", modalidad.Activo);
                    cmd.Parameters.AddWithValue("@id", modalidad.IdModalidad);

                    conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    return filasAfectadas > 0;
                }
            }
        }

        // MÉTODO AUXILIAR (Buscar por Nombre para evitar duplicados)
        public ModalidadTitulacion ObtenerPorNombre(string nombre)
        {
            ModalidadTitulacion modalidad = null;
            using (MySqlConnection conexion = ConexionBD.ObtenerConexion())
            {
                string query = "SELECT * FROM modalidades_titulacion WHERE nombre_modalidad = @nombre";
                using (MySqlCommand cmd = new MySqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    conexion.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modalidad = new ModalidadTitulacion
                            {
                                IdModalidad = Convert.ToInt32(reader["id_modalidad"]),
                                NombreModalidad = reader["nombre_modalidad"].ToString(),
                                Activo = Convert.ToBoolean(reader["activo"])
                            };
                        }
                    }
                }
            }
            return modalidad;
        }
    }
}